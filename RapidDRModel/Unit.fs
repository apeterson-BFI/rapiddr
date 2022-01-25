namespace RapidDRModel

    open System
    open NodaTime

    type Unit(weaponSkills : Skillset, armorSkills : Skillset, survivalSkills : Skillset, statistics : Attributes, vitals : Vitals) = 
        member this.WeaponSkills = weaponSkills

        member this.ArmorSkills = armorSkills

        member this.SurvivalSKills = survivalSkills

        member this.Statistics = statistics

        member this.Vitals = vitals

        member this.TDPSFree = 
            let skfun (sk : Skill) = sk.TDPSEarned
            let wtdps = List.sumBy skfun this.WeaponSkills.Skills
            let atdps = List.sumBy skfun this.ArmorSkills.Skills
            let stdps = List.sumBy skfun this.SurvivalSKills.Skills
            let tdpsSpent = this.Statistics.TDPCost(Unit.DefaultStatistics)

            wtdps + atdps + stdps - tdpsSpent

        member this.Pulse() = 
            this.WeaponSkills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline
            this.ArmorSkills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline
            this.SurvivalSKills.Pulse this.Statistics.Wisdom this.Statistics.Intelligence this.Statistics.Discipline

        member this.Gain(name : string, softCapRank : float, hardCapRank : float, gainBits : float) =
            match this.WeaponSkills.TryFind name with
            | Some sk -> sk.Gain this.Statistics.Intelligence this.Statistics.Discipline softCapRank hardCapRank gainBits
            | None ->
                match this.ArmorSkills.TryFind name with
                | Some sk -> sk.Gain this.Statistics.Intelligence this.Statistics.Discipline softCapRank hardCapRank gainBits
                | None -> 
                    match this.SurvivalSKills.TryFind name with
                    | Some sk -> sk.Gain this.Statistics.Intelligence this.Statistics.Discipline softCapRank hardCapRank gainBits
                    | None -> ()

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
        
        new(proto : Unit) = 
            let na = new Attributes(proto.Statistics)
            let wc = new Skillset(proto.WeaponSkills)
            let ac = new Skillset(proto.ArmorSkills)
            let sc = new Skillset(proto.SurvivalSKills)

            Unit(wc, ac, sc, na, Unit.DefaultVitals)

        static member CreateNewBaseUnit() = Unit(Unit.DefaultWeaponSkills, Unit.DefaultArmorSkills, Unit.DefaultSurvivalSkills, Unit.DefaultStatistics, Unit.DefaultVitals)
            