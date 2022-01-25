namespace RapidDRModel
    open System
    open NodaTime
    open MathNet.Numerics.Distributions

    type DefenseSet = 
        | ShieldDefense of ArmorData * float
        | ParryDefense of WeaponData * float * float

    type ResultTableEntry = 
        | Miss
        | Hit of string * float

    type BattleContext(units : Unit list) = 
        static member private Roll = new ContinuousUniform(0.0, 1.0)

        static member private HitTable = 
            [|
                (* 0 *) Miss;
                (* 1 *) Miss;
                (* 2 *) Miss;
                (* 3 *) Miss;
                (* 4 *) Miss;
                (* 5 *) Miss;
                (* 6 *) Miss;
                (* 7 *) Miss;
                (* 8 *) Hit ("Puny Hit", 0.1);
                (* 9 *) Hit ("Minor Hit", 0.4);
                (* 10 *) Hit ("Slight Hit", 0.7);
                (* 11 *) Hit ("Weak Hit", 1.0);
                (* 12 *) Hit ("Poor Hit", 1.25);
                (* 13 *) Hit ("Average Hit", 1.5);
                (* 14 *) Hit ("Good Hit", 2.0);
                (* 15 *) Hit ("Great Hit", 2.5);
                (* 16 *) Hit ("Astonishing Hit", 3.0);
                (* 17 *) Hit ("Devastating Hit", 3.5);
                (* 18 *) Hit ("Earthshaking Hit", 4.0);
                (* 19 *) Hit ("Apocalyptic Hit", 5.0)
            |]

        member val Units = units with get,set

        // let expStats = 13.942 + 0.055 * rank
        // K stamina + K / 3 strength = HP
        // K 10 + K / 3 10 = 100
        // 4/3 K = 10
        // K = 7.5
        // 7.5 (13.942 + 0.055 rank) + 2.5 (13.942 + 0.055 rank)
        // 139.42 + 0.55 rank
        // base dmg scale = 13.94 + 0.055 rank)

        member private this.ResolveCombat (att_unit : Unit) (def_unit : Unit) (att_weapon : WeaponData) (def_armor : ArmorData) (dset : DefenseSet) (att_stats : Attributes) (def_stats : Attributes) (att_vitals : Vitals) (def_vitals : Vitals) = 
            let att_ranks = att_weapon.WeaponRanks
            let att_rankdmg = 13.94 + 0.055 * att_ranks
            let att_dmgboost = att_weapon.Gear.DamageMod
            let att_accboost = att_weapon.Gear.AccuracyMod
            let att_hppctpenalty = 0.7 + 0.3 * att_vitals.Health / att_vitals.HealthMax
            let att_agilitymod = Math.Sqrt(att_stats.Agility / 30.0)
            let att_strengthmod = Math.Sqrt(att_stats.Strength / 30.0)

            let hit_score = Math.Max(1.0, att_ranks * att_accboost * att_hppctpenalty * att_agilitymod)
            let damage_score = att_rankdmg * att_dmgboost * att_hppctpenalty * att_strengthmod
            
            let evasion_ranks = 
                match dset with
                | ShieldDefense (ad, ev) -> ev
                | ParryDefense (wd, p, ev) -> ev

            let seconddef_ranks = 
                match dset with
                | ShieldDefense (ad, _) -> ad.ArmorRanks
                | ParryDefense (wd, p, ev) -> p
            
            let seconddef_reduction = 
                match dset with
                | ShieldDefense (ad, _) -> ad.Gear.Absorbance
                | ParryDefense (wd, p, ev) -> wd.Gear.AccuracyMod

            let armor_ranks = def_armor.ArmorRanks

            let armor_resultred = def_armor.Gear.ResultReduction
            let armor_absorbance = def_armor.Gear.Absorbance

            let def_reflexmod = Math.Sqrt(def_stats.Reflex)
            let def_staminamod = Math.Sqrt(def_stats.Stamina)

            let def_hppctpenalty = 0.7 + 0.3 * att_vitals.Health / att_vitals.HealthMax

            // Attack vs. Evasion - Gives reduction based on evasion defense roll
            let evasion_def = Math.Max(1.0, evasion_ranks * def_reflexmod * def_hppctpenalty)      
            let evasion_marker = 0.1 * evasion_def / hit_score

            let evroll = BattleContext.Roll.Sample()

            let after_ev_hit_score = 
                if evroll > evasion_marker * 3.0 then hit_score
                elif evroll > evasion_marker * 2.5 then hit_score * 0.90
                elif evroll > evasion_marker * 2.0 then hit_score * 0.80
                elif evroll > evasion_marker * 1.5 then hit_score * 0.70
                elif evroll > evasion_marker * 1.25 then hit_score * 0.60
                elif evroll > evasion_marker * 1.1 then hit_score * 0.5
                else 0.0

            let after_ev_dmg = 
                if evroll > evasion_marker * 5.0 then damage_score
                elif evroll > evasion_marker * 4.0 then damage_score * 0.80
                elif evroll > evasion_marker * 3.0 then damage_score * 0.60
                elif evroll > evasion_marker * 2.0 then damage_score * 0.40
                elif evroll > evasion_marker * 1.5 then damage_score * 0.20
                elif evroll > evasion_marker * 1.25 then damage_score * 0.10
                else 0.0

            let second_def = Math.Max(1.0, seconddef_ranks * def_reflexmod * seconddef_reduction * def_hppctpenalty)

            let tableroll = BattleContext.Roll.Sample()

            let hit_result = 20.0 * tableroll * after_ev_hit_score / second_def - armor_resultred
            
            let resultentry = 
                if hit_result <= 0.0 then BattleContext.HitTable[0]
                elif hit_result >= 20.0 then BattleContext.HitTable[19]
                else BattleContext.HitTable[int hit_result]

            let table_dmgmod = 
                match resultentry with
                | Miss -> 0.0
                | Hit (s, d) -> d

            let final_dmg = after_ev_dmg * table_dmgmod / armor_absorbance

            def_unit.Vitals.Health <- def_unit.Vitals.Health - final_dmg
            ()

        member this.Attack (attackerIdent : string) (defenderIdent : string)  = 
            let (aunito : Unit option) = List.tryFind (fun u -> u.Ident = attackerIdent) this.Units
            let (dunito : Unit option) = List.tryFind (fun u -> u.Ident = defenderIdent) this.Units

            // require att unit, def unit

            match aunito with
            | None -> ()
            | Some aunit ->
                match dunito with
                | None -> ()
                | Some dunit ->
                    match aunit.Vitals.Status, dunit.Vitals.Status with
                    | Alive, Alive ->
                        let attc = aunit.GetUnitCombatData ()
                        let defc = dunit.GetUnitCombatData ()

                        // require att. Weapon data
                        // require def. Armor data (no armor means make 0 skill / protect armor)
                        // require def. Shield data or Weapon data (Shield preemts)
                
                        let def_ad = 
                            match defc.ArmorData with
                            | None -> { ArmorRanks = 0.0; Gear = { Ident = "none"; SkillName = "none"; ResultReduction = 0; Absorbance = 0.0 } }
                            | Some ad' -> ad'

                        match attc.WeaponData with
                        | None -> ()
                        | Some att_wd ->
                            let def_dset = 
                                let shield_dset = Option.map (fun sd -> ShieldDefense (sd, defc.EvasionRanks)) defc.ShieldData
                                let parry_dset = Option.map (fun wd -> ParryDefense (wd, defc.ParryRanks, defc.EvasionRanks)) defc.WeaponData

                                Option.orElse parry_dset shield_dset

                            match def_dset with
                            | None -> ()
                            | Some dset ->
                                this.ResolveCombat aunit dunit att_wd def_ad dset aunit.Statistics dunit.Statistics aunit.Vitals dunit.Vitals
                    | _,_ -> ()
