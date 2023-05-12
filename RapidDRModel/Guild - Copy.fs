namespace RapidDRModel

    open System
    open NodaTime

    type SkillRequirement = 
        | LastBlock of float * float
        | Block of SkillRequirement * float * float

    type SkillsetRequirement = 
        | TopN of SkillRequirement * int
        | TotalN of SkillRequirement

    type GuildRequirements = 
        { weaponReq : SkillsetRequirement; armorReq : SkillsetRequirement; survivalReq : SkillsetRequirement; otherReqs : Map<string, SkillRequirement> } 

    module Guild = 
        let barbReqs =
            { weaponReq = TopN(Block(Block(Block(Block(LastBlock(10.0, 4.0), 20.0, 5.0), 40.0, 6.0), 30.0, 8.0), 50.0, 10.0), 4);
              armorReq = TopN(Block(Block(Block(Block(LastBlock(10.0, 3.0), 20.0, 4.0), 40.0, 5.0), 30.0, 6.0), 50.0, 8.0), 2);
              survivalReq = TopN(Block(Block(Block(Block(LastBlock(10.0, 2.0), 20.0, 2.0), 40.0, 3.0), 30.0, 4.0), 50.0, 5.0), 1);
              otherReqs = Map [ ("Parry", Block(Block(Block(Block(LastBlock(10.0, 4.0), 20.0, 4.0), 40.0, 4.0), 30.0, 4.0), 50.0, 5.0));
                                ("Evasion", Block(Block(Block(Block(LastBlock(10.0, 4.0), 20.0, 4.0), 40.0, 4.0), 30.0, 4.0), 50.0, 5.0)) ]
            }