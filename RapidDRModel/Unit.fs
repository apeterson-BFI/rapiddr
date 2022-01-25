namespace RapidDRModel

    open System
    open NodaTime

    type UnitType = 
        | PC
        | Monster
        | NPC

    type UnitCombatData = 
        { ShieldData : ArmorData option; ArmorData : ArmorData option; WeaponData : WeaponData option; VitalData : Vitals; Statistics : Attributes; EvasionRanks : float; ParryRanks : float }

    type Unit(ident : string, weaponSkills : Skillset, armorSkills : Skillset, survivalSkills : Skillset, statistics : Attributes, vitals : Vitals, guildReqs : GuildRequirements, equipped : Portrait, unitType : UnitType) = 
 
        let rec findLevelSatisfied (req : SkillRequirement) (ability : float) = 
            match req with
            | LastBlock (n, rql) -> Math.Min(n, Math.Floor(ability / rql))
            | Block (rq2, n, rql) ->
                if ability > n * rql
                then n + findLevelSatisfied rq2 (ability - n * rql) 
                else Math.Floor(ability / rql)

        let findTopNLevelSatisfied (req : SkillRequirement) (n : int) (skills : Skillset) = 
            skills.Skills
            |> List.map (fun (sk : Skill) -> sk.SkillRanks)
            |> List.map (findLevelSatisfied req)
            |> List.sortByDescending id
            |> List.item (n - 1)

        let findTotalNLevelSatisfied (req : SkillRequirement) (skills : Skillset) = 
            skills.Skills
            |> List.sumBy (fun (sk : Skill) -> sk.SkillRanks)
            |> findLevelSatisfied req

        let findSkillsetLevelSatisfied (sreq : SkillsetRequirement) (skills : Skillset) = 
            match sreq with
            | TopN (s, n) -> findTopNLevelSatisfied s n skills
            | TotalN (s) -> findTotalNLevelSatisfied s skills            

        member this.FindLevelFromReqs () = 
            let wlevel = findSkillsetLevelSatisfied this.GuildRequirements.weaponReq weaponSkills
            let alevel = findSkillsetLevelSatisfied this.GuildRequirements.armorReq armorSkills
            let slevel = findSkillsetLevelSatisfied this.GuildRequirements.survivalReq survivalSkills

            let findNamedSkillLevelSatisfied (req : SkillRequirement) (skillName : string) = 
                match this.TryFind skillName with
                | Some (sk : Skill) -> findLevelSatisfied req sk.SkillRanks
                | None -> 0.0

            let olevels = 
                this.GuildRequirements.otherReqs
                |> Map.toList
                |> List.map (fun (nm,req) -> findNamedSkillLevelSatisfied req nm)
                |> List.min

            List.min [wlevel; alevel; slevel; olevels]     

        member this.WeaponSkills = weaponSkills

        member this.ArmorSkills = armorSkills

        member this.SurvivalSKills = survivalSkills

        member this.Statistics = statistics

        member this.Vitals = vitals

        member this.GuildRequirements = guildReqs

        member val Equipped = equipped with get,set

        member this.UnitType = unitType

        member val Ident = ident with get,set

        // V!: Armor goes on Torso. Shield on Left shoulder or Left hand
        //     Weapon in right hand

        member this.GetShield () = 
            let lh = 
                match this.Equipped.LeftHand with
                | ArmorSlot arm ->
                    if arm.SkillName = "Shield" then Some (arm)
                    else None
                | WeaponSlot _ -> None
                | Container _ -> None
                | Nothing -> None

            let ls = 
                match this.Equipped.LeftShoulder with
                | ArmorSlot arm ->
                    if arm.SkillName = "Shield" then Some (arm)
                    else None
                | WeaponSlot _ -> None
                | Container _ -> None
                | Nothing -> None

            Option.orElse ls lh

        member this.HasShield = 
            this.GetShield () 
            |> Option.isSome

        member this.GetWeapon () = 
            match this.Equipped.RightHand with
            | ArmorSlot _ -> None
            | WeaponSlot wp -> Some wp
            | Container _ -> None
            | Nothing -> None

        member this.HasWeapon = 
            match this.Equipped.RightHand with
            | ArmorSlot _ -> false
            | WeaponSlot _ -> true
            | Container _ -> false
            | Nothing -> false

        member this.GetArmor () = 
            match this.Equipped.Torso with
            | ArmorSlot ar -> if ar.SkillName = "Shield" then None else Some ar
            | WeaponSlot _ -> None
            | Container _ -> None
            | Nothing -> None

        member this.HasArmor =
            this.GetArmor()
            |> Option.isSome

        member this.Evasion
            with get() = 
                match this.SurvivalSKills.TryFind("Evasion") with
                | None -> 0.0
                | Some ev -> ev.SkillRanks

        member this.Parry
            with get() = 
                match this.WeaponSkills.TryFind("Parry") with
                | None -> 0.0
                | Some p -> p.SkillRanks

        member this.WeaponData () = 
            let wsloto = this.GetWeapon ()

            match wsloto with
            | Some wslot ->
                match this.WeaponSkills.TryFind wslot.SkillName with
                | None -> None
                | Some (sk : Skill) -> Some { WeaponRanks = sk.SkillRanks; Gear = wslot }
            | None -> None

        member this.ArmorData () = 
            let asloto = this.GetArmor ()

            match asloto with
            | Some aslot ->
                match this.ArmorSkills.TryFind aslot.SkillName with
                | None -> None
                | Some (sk : Skill) -> Some { ArmorRanks = sk.SkillRanks; Gear = aslot }
            | None -> None

        member this.ShieldData () = 
            let ssloto = this.GetShield ()

            match ssloto with
            | Some sslot ->
                match this.ArmorSkills.TryFind "Shield" with
                | None -> None
                | Some (sk : Skill) -> Some { ArmorRanks = sk.SkillRanks; Gear = sslot }
            | None -> None

        member this.TDPSFree = 
            let skfun (sk : Skill) = sk.TDPSEarned
            let wtdps = List.sumBy skfun this.WeaponSkills.Skills
            let atdps = List.sumBy skfun this.ArmorSkills.Skills
            let stdps = List.sumBy skfun this.SurvivalSKills.Skills

            let level = this.FindLevelFromReqs()
            let ltdps = 100.0 * level + level * (level + 1.0) / 2.0

            let tdpsSpent = this.Statistics.TDPCost(Unit.DefaultStatistics)

            wtdps + atdps + stdps + ltdps - tdpsSpent

        member this.Pulse() = 
            this.WeaponSkills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline
            this.ArmorSkills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline
            this.SurvivalSKills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline

        member this.Gain(name : string, softCapRank : float, hardCapRank : float, gainBits : float) =
            match this.TryFind name with
            | Some (sk : Skill) -> sk.Gain this.Statistics.Intelligence this.Statistics.Discipline softCapRank hardCapRank gainBits
            | None -> ()

        member this.TryFind(name : string) = 
            match this.WeaponSkills.TryFind name with
            | Some sk -> Some sk
            | None ->
                match this.ArmorSkills.TryFind name with
                | Some sk -> Some sk
                | None -> 
                    match this.SurvivalSKills.TryFind name with
                    | Some sk -> Some sk
                    | None -> None

        member this.GetUnitCombatData () = 
            { ShieldData = this.ShieldData (); ArmorData = this.ArmorData (); WeaponData = this.WeaponData (); Statistics = this.Statistics; 
              VitalData = this.Vitals; EvasionRanks = this.Evasion; ParryRanks = this.Parry  }
            
        member this.UpdateMaxHealth () =
            let newmax = 7.5 * this.Statistics.Stamina + 2.5 * this.Statistics.Strength
            let delta = newmax - this.Vitals.HealthMax
            
            this.Vitals.Health <- this.Vitals.Health + delta
            this.Vitals.HealthMax <- newmax

        member this.UpdateMaxEndurance () = 
            let newmax = 7.5 * this.Statistics.Stamina + 2.5 * this.Statistics.Discipline
            let delta = newmax - this.Vitals.EnduranceMax

            this.Vitals.Endurance <- this.Vitals.Endurance + delta
            this.Vitals.EnduranceMax <- newmax

        member this.AdjustHealth (hpDelta : float) =
            this.Vitals.Health <- this.Vitals.Health + hpDelta

        member this.AdjustEndurance (enduDelta : float) = 
            this.Vitals.Endurance <- this.Vitals.Endurance + enduDelta

        member this.TryTiring (enduDelta : float) =
            if this.Vitals.Endurance + enduDelta < 0.0 then false
            else this.AdjustEndurance enduDelta; true

        static member DefaultStatistics = new Attributes(10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0)

        static member DefaultWeaponSkills = 
            new Skillset("Weapon", 
                [new Skill("Light Edged", 0.0, Skill.Primary, 0.0);
                 new Skill("Medium Edged", 0.0, Skill.Primary, 0.0);
                 new Skill("Heavy Edged", 0.0, Skill.Primary, 0.0);
                 new Skill("Two-Handed Edged", 0.0, Skill.Primary, 0.0);
                 new Skill("Light Blunt", 0.0, Skill.Primary, 0.0);
                 new Skill("Medium Blunt", 0.0, Skill.Primary, 0.0);
                 new Skill("Heavy Blunt", 0.0, Skill.Primary, 0.0);
                 new Skill("Two-Handed Blunt", 0.0, Skill.Primary, 0.0);
                 new Skill("Brawling", 0.0, Skill.Primary, 0.0);
                 new Skill("Short Bow", 0.0, Skill.Primary, 0.0);
                 new Skill("Long Bow", 0.0, Skill.Primary, 0.0);
                 new Skill("Composite Bow", 0.0, Skill.Primary, 0.0);
                 new Skill("Light Crossbow", 0.0, Skill.Primary, 0.0);
                 new Skill("Heavy Crossbow", 0.0, Skill.Primary, 0.0);
                 new Skill("Multiple Opponents", 0.0, Skill.Primary, 0.0);
                 new Skill("Sling", 0.0, Skill.Primary, 0.0);
                 new Skill("Staff Sling", 0.0, Skill.Primary, 0.0);
                 new Skill("Halberd", 0.0, Skill.Primary, 0.0);
                 new Skill("Pike", 0.0, Skill.Primary, 0.0);
                 new Skill("Parry", 0.0, Skill.Primary, 0.0)],
                 Skill.Primary)

        static member DefaultArmorSkills = 
            new Skillset("Armor",
                [new Skill("Leather Armor", 0.0, Skill.Secondary, 0.0);
                 new Skill("Light Chain Armor", 0.0, Skill.Secondary, 0.0);
                 new Skill("Heavy Chain Armor", 0.0, Skill.Secondary, 0.0);
                 new Skill("Light Plate Armor", 0.0, Skill.Secondary, 0.0);
                 new Skill("Heavy Plate Armor", 0.0, Skill.Secondary, 0.0);
                 new Skill("Shield", 0.0, Skill.Secondary, 0.0)],
                 Skill.Secondary)

        static member DefaultSurvivalSkills = 
            new Skillset("Survival",
                [new Skill("Evasion", 0.0, Skill.Secondary, 0.0)], Skill.Secondary)

        static member DefaultVitals = 
            new Vitals(100.0, 100.0, 100.0, 0.0, Alive)

        static member DefaultPortrait = 
            { Back = Nothing; LeftShoulder = Nothing; RightShoulder = Nothing; LeftHand = Nothing; RightHand = Nothing;
              Torso = Nothing }
                
        static member DefaultIdent = "Blank"
            
        new(proto : Unit) = 
            let na = new Attributes(proto.Statistics)
            let wc = new Skillset(proto.WeaponSkills)
            let ac = new Skillset(proto.ArmorSkills)
            let sc = new Skillset(proto.SurvivalSKills)

            Unit(Unit.DefaultIdent, wc, ac, sc, na, Unit.DefaultVitals, proto.GuildRequirements, Unit.DefaultPortrait, proto.UnitType)

        static member CreateNewBaseUnit() = Unit(Unit.DefaultIdent, Unit.DefaultWeaponSkills, Unit.DefaultArmorSkills, Unit.DefaultSurvivalSkills, Unit.DefaultStatistics, Unit.DefaultVitals, Guild.barbReqs, Unit.DefaultPortrait, UnitType.PC)
            