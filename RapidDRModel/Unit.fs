namespace RapidDRModel

    open System
    open NodaTime

    type Unit(weaponSkills : Skillset, armorSkills : Skillset, survivalSkills : Skillset, statistics : Attributes, vitals : Vitals, guildReqs : GuildRequirements) = 
 
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

            Unit(wc, ac, sc, na, Unit.DefaultVitals, proto.GuildRequirements)

        static member CreateNewBaseUnit() = Unit(Unit.DefaultWeaponSkills, Unit.DefaultArmorSkills, Unit.DefaultSurvivalSkills, Unit.DefaultStatistics, Unit.DefaultVitals, Guild.barbReqs)
            