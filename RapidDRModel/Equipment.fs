namespace RapidDRModel
    open System
    open NodaTime

    // Expect weapon SkillName in Weapon skills list
    type Weapon = 
        { Ident : string; SkillName : string; DamageMod : float; AccuracyMod : float; EnduranceCost : float; }


    // Expect armor SkillName in Armor skills list
    // ResultReduction - flat always effective reduction in Damage results damage
    // Absorbance - damage is reduced by this factor, subject to armor skill. Greater than 1, larger is better.
    type Armor = 
        { Ident : string; SkillName : string; ResultReduction : float; Absorbance : float; }

    type Equipment = 
        | WeaponSlot of Weapon
        | ArmorSlot of Armor
        | Container of Equipment list
        | Nothing

    type Portrait =
        { Back : Equipment;
          LeftShoulder : Equipment; 
          RightShoulder : Equipment;
          LeftHand : Equipment;
          RightHand : Equipment;
          Torso : Equipment }

     type ArmorData = 
        { ArmorRanks : float; Gear : Armor }

     type WeaponData = 
        { WeaponRanks : float; Gear : Weapon }



