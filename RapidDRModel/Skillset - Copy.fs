﻿namespace RapidDRModel

    open System
    open NodaTime
    open System.Collections.Generic
    
    type Skillset(name : string, skills : Skill list, skillsetType : SkillsetPoolParameters) = 
        member this.Name = name
        
        member this.Skills = skills
        
        member this.SkillsetType = skillsetType

        new(skillset : Skillset) = 
            let skwipe = List.map (fun sk -> Skill(sk)) skillset.Skills
            Skillset(skillset.Name, skwipe, skillset.SkillsetType)

        member this.Pulse (queue : Collections.Generic.Queue<string>) (wisdom : float) (intel : float) (disc : float) =
            List.iter (fun (x : Skill) -> x.Pulse queue wisdom intel disc) this.Skills 

        member this.TryFind (name : string) = 
            List.tryFind (fun (x : Skill) -> x.Name = name) this.Skills

        member this.SetAllSkillsRank (rank : float) = 
            List.iter (fun (sk : Skill) -> sk.SetRank(rank)) this.Skills
