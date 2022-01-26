namespace RapidDRModel
    open System
    open NodaTime
    open MathNet.Numerics.Distributions

    type Cap = { SoftCap : float; HardCap : float }

    type Monster(prefix : string, unit : Unit,  loot : float, weaponCap : Cap, armorCap : Cap, defensesCap : Cap, expgain : float) = 
        static member val SequenceNumber = 1 with get,set
        
        member this.IdentPrefix = prefix

        member this.Unit = unit

        member this.Loot = loot

        member this.WeaponCap = weaponCap

        member this.ArmorCap = armorCap
        
        member this.DefensesCap = defensesCap

        member this.ExpGain = expgain

        member this.Spawn () = 
            let u = new Unit(this.Unit)
            u.Ident <- this.IdentPrefix + " " + (string Monster.SequenceNumber)
            Monster.SequenceNumber <- Monster.SequenceNumber + 1
            u

    type MonsterSpawner(baseMonster : Monster, battleContext : BattleContext, spawnChance : float, monsterLimit : int, simParams : SimParams) = 
        static member private Roll = new ContinuousUniform(0.0, 1.0)

        member this.BaseMonster = baseMonster

        member this.BattleContext = battleContext

        member this.SpawnChance = spawnChance

        member this.MonsterLimit = monsterLimit

        member this.SimParams = simParams

        member this.TrySpawning () = 
            let roll = MonsterSpawner.Roll.Sample()

            if roll < this.SpawnChance
            then
                let currmon = 
                    this.BattleContext.Units
                    |> List.filter (fun (u : Unit) -> u.UnitType = UnitType.Monster)
                    |> List.length
                
                if currmon < this.MonsterLimit
                then
                    this.BattleContext.Join(this.BaseMonster.Spawn())
                else
                    ()
            else
                ()

        member this.CheckSituation () = 
            // Only supports one adventurer currently
            // Gain even for armor, and defenses is only on kill
            let adventuo = 
                this.BattleContext.Units
                |> List.filter (fun (u : Unit) -> u.UnitType = UnitType.PC)
                |> List.tryHead

            this.BattleContext.Units
            |> List.filter (fun (u : Unit) -> u.Vitals.Status = Alive)
            |> List.iter (fun (u : Unit) ->
                            u.AdjustHealth (simParams.HealthPctRecovery * u.Vitals.HealthMax)
                            u.AdjustEndurance (simParams.EndurancePctRecovery * u.Vitals.EnduranceMax))

            match adventuo with
            | Some (adventu : Unit) ->
                let monstuo = 
                    this.BattleContext.Units
                    |> List.filter (fun (u : Unit) -> u.UnitType = UnitType.Monster)
                    |> List.filter (fun (u : Unit) -> u.Vitals.Status = Alive)
                    |> List.tryLast

                match monstuo with
                | None -> ()
                | Some monstu ->
                    match adventu.GetWeapon () with
                    | None -> ()
                    | Some (wd) ->                    
                        if adventu.Vitals.Endurance >= wd.EnduranceCost
                        then
                            this.BattleContext.Attack adventu.Ident monstu.Ident
                        else
                            ()

                this.BattleContext.Units
                |> List.filter (fun (u : Unit) -> u.UnitType = UnitType.Monster)
                |> List.filter (fun (u : Unit) -> u.Vitals.Status <> Annihilated)                
                |> List.iter (fun (u : Unit) -> 
                                match u.GetWeapon () with
                                | None -> ()
                                | Some (wd) -> 
                                    if u.Vitals.Endurance >= wd.EnduranceCost
                                    then
                                        this.BattleContext.Attack u.Ident adventu.Ident
                                    else
                                        ()
                                )

                this.BattleContext.Units
                |> List.filter (fun (u : Unit) -> u.UnitType = UnitType.Monster)
                |> List.filter (fun (u : Unit) -> u.Vitals.Status = Annihilated)
                |> List.iter
                    (fun (u : Unit) ->
                        adventu.GainActiveWeapon baseMonster.ExpGain baseMonster.WeaponCap.SoftCap baseMonster.WeaponCap.HardCap
                        adventu.GainActiveArmor baseMonster.ExpGain baseMonster.ArmorCap.SoftCap baseMonster.ArmorCap.HardCap
                        adventu.GainActiveShield baseMonster.ExpGain baseMonster.DefensesCap.SoftCap baseMonster.DefensesCap.HardCap
                        adventu.GainActiveParry baseMonster.ExpGain baseMonster.DefensesCap.SoftCap baseMonster.DefensesCap.HardCap
                        adventu.GainEvasion baseMonster.ExpGain baseMonster.DefensesCap.SoftCap baseMonster.DefensesCap.HardCap
                        )

                this.BattleContext.Units <- this.BattleContext.Units |> List.filter (fun (u : Unit) -> u.Vitals.Status <> Annihilated)
            | None -> ()
