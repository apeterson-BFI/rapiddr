namespace RapidDRModel
    open System
    open NodaTime
    open System.Threading

    module Overseer = 
        let createMonsterRoom (monsterName : string) (roomName : string) (skillLevel : float) (statLevel : float) (weapon : Weapon) (armor : Armor) (shield : Armor option) (loot : float) (expgain : float) (offSoftCap : float) (offHardCap : float) (defSoftCap : float) (defHardCap : float) (spawnChance : float) (monsterLimit : int) (simParams : SimParams) = 
            


            let battleContext = new BattleContext( [] )
            let room = new Room(Map [], roomName, roomName, RoomContents.BattleRoom (battleContext))
            let unit = new Unit(monsterName, Unit.DefaultWeaponSkills, Unit.DefaultArmorSkills, Unit.DefaultSurvivalSkills, new Attributes(statLevel, statLevel, statLevel, statLevel, statLevel, statLevel, statLevel, statLevel), Unit.DefaultVitals, Guild.barbReqs, 
                                { LeftShoulder = Nothing; RightShoulder = Nothing; LeftHand = shield |> Option.map (fun s -> ArmorSlot(s)) |> Option.defaultValue Nothing; RightHand = WeaponSlot (weapon); 
                                  Back = Nothing; Torso = ArmorSlot (armor) }, UnitType.Monster)

            unit.WeaponSkills.SetAllSkillsRank(skillLevel)
            unit.ArmorSkills.SetAllSkillsRank(skillLevel)
            unit.SurvivalSKills.SetAllSkillsRank(skillLevel)
            unit.UpdateMaxHealth()
            unit.UpdateMaxEndurance()

            let monster = new Monster(monsterName, unit, loot, { SoftCap = offSoftCap; HardCap = offHardCap}, { SoftCap = defSoftCap; HardCap = defHardCap}, { SoftCap = defSoftCap; HardCap = defHardCap}, expgain)
            let spawner = new MonsterSpawner(monster, battleContext, spawnChance, monsterLimit, simParams)

            (spawner, room)

        let ratarmor = { Ident = "Rat Armor"; Absorbance = 1.0; ResultReduction = 0.0; SkillName = "Leather" }
        let jerkin = { Ident = "Leather Jerkin"; Absorbance = 1.02; ResultReduction = 0.1; SkillName = "Leather" }
        let cougararmor = { Ident = "Cougar Armor"; Absorbance = 1.04; ResultReduction = 0.1; SkillName = "Leather" }
        let cuir_coat = {Ident = "Leather Coat"; Absorbance = 1.1; ResultReduction = 0.3; SkillName = "Leather" }
        let eelarmor = { Ident = "Eel Armor"; Absorbance = 1.06; ResultReduction = 0.1; SkillName = "Leather" }
        let chain_coat = { Ident = "Chain Coat"; Absorbance = 1.25; ResultReduction = 0.5; SkillName = "Light Chain" }
        let beissarmor = { Ident = "Beisswurm Armor"; Absorbance = 1.1; ResultReduction = 0.3; SkillName = "Leather" }
        let bloodwolfarmor = { Ident = "Blood Wolf Armor"; Absorbance = 1.15; ResultReduction = 0.4; SkillName = "Leather" }
        let chain_armor = { Ident = "Chain Armor"; Absorbance = 1.35; ResultReduction = 0.7; SkillName = "Light Chain"}
        let snowbeastarmor = { Ident = "Snowbeast Armor"; Absorbance = 1.25; ResultReduction = 0.5; SkillName = "Leather"}

        let claw = { Ident = "Claws"; DamageMod = 1.0; AccuracyMod = 1.0; EnduranceCost = 20.0; SkillName = "Brawling" }
        let fist = { Ident = "Fists"; DamageMod = 1.05; AccuracyMod = 1.025; EnduranceCost = 20.0; SkillName = "Brawling" }
        let claw2 = { Ident = "Claws 2"; DamageMod = 1.1; AccuracyMod = 1.05; EnduranceCost = 20.0; SkillName = "Brawling" }
        let bite = { Ident = "Bite"; DamageMod = 1.6; AccuracyMod = 1.2; EnduranceCost = 30.0; SkillName = "Brawling" }
        let rake = {Ident = "Rake"; DamageMod = 1.2; AccuracyMod = 1.1; EnduranceCost = 20.0; SkillName = "Brawling" }

        let shortsword = { Ident = "Short Sword"; DamageMod = 1.10; AccuracyMod = 1.05; EnduranceCost = 20.0; SkillName = "Light Edged" }
        let staff = { Ident = "Staff"; DamageMod = 1.2; AccuracyMod = 1.1; EnduranceCost = 30.0; SkillName = "Staff"}
        let club = { Ident = "Club"; DamageMod = 2.5; AccuracyMod = 1.0; EnduranceCost = 40.0; SkillName = "Heavy Blunt"}
        let scimitar = { Ident = "Scimitar"; DamageMod = 1.6; AccuracyMod = 1.2; EnduranceCost = 30.0; SkillName = "Medium Edged"}
        let warhammer = { Ident = "Warhammer"; DamageMod = 3.5; AccuracyMod = 1.1; EnduranceCost = 60.0; SkillName = "Heavy Blunt"}

        let target_shield = {Ident = "Target Shield"; Absorbance = 1.25; ResultReduction = 0.0; SkillName = "Shield" }
        let medium_shield = {Ident = "Medium Shield"; Absorbance = 1.5; ResultReduction = 0.0; SkillName = "Shield" }

        let sparams = new SimParams(Duration.FromSeconds 3.0, Duration.FromSeconds 20.0, Duration.FromSeconds 1.0)

        let (ratspawn, ratroom) = createMonsterRoom "Rat" "Rat Room" 2.0 4.0 claw ratarmor None 1.0 400.0 15.0 25.0 20.0 30.0 0.05 4 sparams
        let (unarmedgobspawn, unarmedgobroom) = createMonsterRoom "Unarmed Goblin" "Unarmed Goblin Room" 10.0 6.0 fist jerkin None 3.0 440.0 25.0 35.0 32.0 42.0 0.05 4 sparams
        let (armedgobspawn, armedgobroom) = createMonsterRoom "Armed Goblin" "Armed Goblin Room" 15.0 7.0 shortsword jerkin (Some (target_shield)) 3.5 460.0 30.0 40.0 37.0 47.0 0.05 4 sparams
        let (cougarspawn, cougarroom) = createMonsterRoom "Cougar" "Cougar Room" 25.0 8.0 claw2 cougararmor None 5.0 480.0 45.0 55.0 50.0 60.0 0.05 4 sparams
        let (woodtrollspawn, woodtrollroom) = createMonsterRoom "Wood Troll" "Wood Troll Room" 30.0 9.0 club cuir_coat (Some (medium_shield)) 6.0 500.0 50.0 60.0 57.0 67.0 0.03 4 sparams
        let (eelspawn, eelroom) = createMonsterRoom "Eel" "Eel Room" 40.0 10.0 claw2 eelarmor None 8.0 520.0 60.0 70.0 67.0 77.0 0.05 4 sparams
        let (reaverspawn, reaverroom) = createMonsterRoom "Faenrae Reaver" "Faenrae Reaver Room" 55.0 11.0 scimitar chain_coat (Some (target_shield)) 10.0 540.0 65.0 75.0 72.0 82.0 0.05 4 sparams
        let (beisswurmspawn, beisswurmroom) = createMonsterRoom "Beisswurm" "Beisswurm Room" 65.0 12.0 bite beissarmor None 13.0 560.0 75.0 85.0 82.0 92.0 0.05 4 sparams
        let (bloodwolfspawn, bloodwolfroom) = createMonsterRoom "Blood Wolf" "Blood Wolf Room" 75.0 13.0 claw2 bloodwolfarmor None 16.0 580.0 85.0 95.0 92.0 102.0 0.05 4 sparams
        let (rocktrollspawn, rocktrollroom) = createMonsterRoom "Rock Troll" "Rock Troll Room" 90.0 14.0 warhammer chain_armor (Some (medium_shield)) 20.0 600.0 95.0 105.0 105.0 115.0 0.05 4 sparams
        let (snowbeastspawn, snowbeastroom) = createMonsterRoom "Snowbeast" "Snowbeast Room" 95.0 14.0 rake snowbeastarmor None 22.0 620.0 100.0 110.0 110.0 120.0 0.05 4 sparams

        let westgateroom = new Room(Map [], "West Gate", "West Gate", TravelRoom)
        let introom = new Room(Map [], "Int Training Room", "Int Training Room", Shop (StatTrainer ("Intelligence")))
        let wisroom = new Room(Map [], "Wis Training Room", "Wis Training Room", Shop (StatTrainer ("Wisdom")))
        let charoom = new Room(Map [], "Cha Training Room", "Cha Training Room", Shop (StatTrainer ("Charisma")))
        let agiroom = new Room(Map [], "Agi Training Room", "Agi Training Room", Shop (StatTrainer ("Agility")))
        let refroom = new Room(Map [], "Ref Training Room", "Ref Training Room", Shop (StatTrainer ("Reflex")))
        let disroom = new Room(Map [], "Dis Training Room", "Dis Training Room", Shop (StatTrainer ("Discipline")))
        let strroom = new Room(Map [], "Str Training Room", "Str Training Room", Shop (StatTrainer ("Strength")))
        let staroom = new Room(Map [], "Sta Training Room", "Sta Training Room", Shop (StatTrainer ("Stamina")))

        let eastgateroom = new Room(Map [], "East Gate", "East Gate", TravelRoom)
        let paladinroom = new Room(Map [], "Paladin Hall", "Paladin Hall", TravelRoom)
        let bridgeroom = new Room(Map [], "Bridge", "Bridge", TravelRoom)

        // leather - jerkin / coat / full leather - 1.05 to 1.25 protection, 0 - 0.5 reduction, 30 - 300 loot
        // light chain - shirt / suit / full light chain - 1.10 to 1.50 protection, 0.5 - 1.0 reduction, 100 - 1000 loot
        // heavy chain - shirt / suit / full heavy chain - 1.20 to 2.00 protection, 1.0 - 1.5 reduction, 300 - 3000 loot
        // light plate - breastplate / suit / full light plate - 1.40 to 3.00 protection, 1.5 - 2.0 reduction, 600 - 6000 loot
        // heavy plate - breastplate / suit / full heavy plate - 2.00 to 6.00 protection, 2.0 - 2.5 reduction, 1000 - 10000 loot

        // leather - leather coat - 1.15 protection, 0.2 reduction, 30 loot
        let pl_leather_coat = { Ident = "Leather Coat"; Absorbance = 1.15; ResultReduction = 0.2; SkillName = "Leather" }
        // light chain - light chain suit - 1.30 protection, 0.6 reduction, 100 loot
        let pl_light_chain_suit = { Ident = "Light Chain Suit"; Absorbance = 1.30; ResultReduction = 0.6; SkillName = "Light Chain" }
        // heavy chain - heavy chain armor - 1.60 protection, 1.1 reduction, 200 loot
        let pl_heavy_chain_armor = { Ident = "Heavy Chain Armor"; Absorbance = 1.60; ResultReduction = 1.1; SkillName = "Heavy Chain" }
        // light plate - light plate breastplate - 2.00 protection, 1.6 reduction, 350 loot
        let pl_light_plate_armor = { Ident = "Light Plate Breastplate"; Absorbance = 2.00; ResultReduction = 1.6; SkillName = "Light Plate" }
        // heavy plate - heavy plate armor - 2.50 protection, 2.1 reduction, 500 loot
        let pl_heavy_plate_armor = { Ident = "Heavy Plate Armor"; Absorbance = 2.50; ResultReduction = 2.1; SkillName = "Heavy Plate" }

        let barbroom = new Room(Map [], "Barbarian Hall", "Barbarian Hall", RoomContents.Guild (GuildLeveler (Guild.barbReqs)))
        let armorroom = new Room(Map [], "Tembeg's Armor", "Tembeg's Armor", 
                            RoomContents.Shop (EquipmentList 
                                ([
                                  (30.0, ArmorSlot (pl_leather_coat));
                                  (100.0, ArmorSlot (pl_light_chain_suit));
                                  (200.0, ArmorSlot (pl_heavy_chain_armor));
                                  (350.0, ArmorSlot (pl_light_plate_armor));
                                  (500.0, ArmorSlot (pl_heavy_plate_armor));
                                  (20.0, Container ( [] ))
                                ])))
        
        // light edged - short sword - 10 end cost, 0.50 damage mod, 1.15 hit mod, 20 loot 
        let pl_short_sword = { Ident = "Short Sword"; DamageMod = 0.5; AccuracyMod = 1.15; SkillName = "Light Edged"; EnduranceCost = 10.0 }
        // medium edged - scimitar - 20 end cost, 1.00 damage mod, 1.10 hit mod, 40 loot
        let pl_scimitar = { Ident = "Scimitar"; DamageMod = 1.0; AccuracyMod = 1.1; SkillName = "Medium Edged"; EnduranceCost = 20.0 }
        // heavy edged - broadsword - 30 end cost, 1.60 damage mod, 1.05 hit mod, 60 loot 
        let pl_broadsword = { Ident = "Broadsword"; DamageMod = 1.6; AccuracyMod = 1.05; SkillName = "Heavy Edged"; EnduranceCost = 30.0 }
        // two-handed edged - greatsword - 60 end cost, 3.5 damage mod, 1.00 hit mod, 100 loot
        let pl_greatsword = { Ident = "Greatsword"; DamageMod = 3.5; AccuracyMod = 1.00; SkillName = "Two-Handed Edged"; EnduranceCost = 60.0 }
        
        // light blunt - cudgel - 20 end cost, 1.20 damage mod, 0.95 hit mod, 50 loot
        let pl_cudgel = { Ident = "Cudgel"; DamageMod = 1.20; AccuracyMod = 0.95; SkillName = "Light Blunt"; EnduranceCost = 20.0 }
        // medium blunt - hammer - 40 end cost, 2.50 damage mod, 0.90 hit mod, 75 loot
        let pl_hammer = { Ident = "Hammer"; DamageMod = 2.5; AccuracyMod = 0.9; SkillName = "Medium Blunt"; EnduranceCost = 40.0 }
        // heavy blunt - ball and chain - 60 end cost, 3.80 damage mod, 0.85 hit mod, 100 loot
        let pl_ballandchain = { Ident = "Ball and Chain"; DamageMod = 3.0; AccuracyMod = 0.85; SkillName = "Heavy Blunt"; EnduranceCost = 60.0 }
        // two-handed blunt - war mattock - 80 end cost, 6.00 damage mod, 0.75 hit mod, 125 loot
        let pl_warmattock = { Ident = "War Mattock"; DamageMod = 6.0; AccuracyMod = 0.75; SkillName = "Two-Handed Blunt"; EnduranceCost = 80.0 }

        // brawling - gauntlet - 30 end cost, 1.50 damage mod, 1.00 hit mod, 30 loot
        let pl_gauntlet = { Ident = "Gauntlet"; DamageMod = 1.50; AccuracyMod = 1.00; SkillName = "Brawling"; EnduranceCost = 30.0 }
        // short bow - short bow - 40 end cost, 1.80 damage mod, 1.20 hit mod, 30 loot
        let pl_shortbow = { Ident = "Short Bow"; DamageMod = 1.80; AccuracyMod = 1.20; SkillName = "Short Bow"; EnduranceCost = 40.0 }
        // long bow - longbow - 60 end cost, 3.00 damage mod, 1.00 hit mod, 60 loot
        let pl_longbow = { Ident = "Longbow"; DamageMod = 3.00; AccuracyMod = 1.00; SkillName = "Longbow"; EnduranceCost = 60.0 }
        // composite bow - composite bow - 80 end cost, 4.20 damage mod, 0.95 hit mod, 80 loot
        let pl_compbow = { Ident = "Composite Bow"; DamageMod = 4.20; AccuracyMod = 0.95; SkillName = "Composite Bow"; EnduranceCost = 80.0 }
        // light crossbow - light crossbow - 70 end cost, 3.75 damage mod, 0.95 hit mod, 70 loot
        let pl_lightxbow = { Ident = "Light Crossbow"; DamageMod = 3.75; AccuracyMod = 0.95; SkillName = "Light Crossbow"; EnduranceCost = 70.0 }
        // heavy crossbow - heavy crossbow - 100 end cost, 6.00 damage mod, 0.90 hit mod, 125 loot
        let pl_heavyxbow = { Ident = "Heavy Crossbow"; DamageMod = 6.00; AccuracyMod = 0.90; SkillName = "Heavy Crossbow"; EnduranceCost = 100.0 }
        // sling - sling - 40 end cost, 2.00 damage mod, 0.90 hit mod, 10 loot
        let pl_sling = { Ident = "Sling"; DamageMod = 2.00; AccuracyMod = 0.90; SkillName = "Sling"; EnduranceCost = 40.0 }
        // staff sling - staff sling - 60 end cost, 3.00 damage mod, 0.80 hit mod, 25 loot
        let pl_staffsling = { Ident = "Staff Sling"; DamageMod = 3.00; AccuracyMod = 0.80; SkillName = "Staff Sling"; EnduranceCost = 60.0 }
        // halberd - halberd - 40 end cost, 1.50 damage mod, 1.25 hit mod, 80 loot
        let pl_halberd = { Ident = "Halberd";  DamageMod = 1.50; AccuracyMod = 1.25; SkillName = "Staff"; EnduranceCost = 40.0 }
        // pike - pike - 40 end cost, 1.00 damage mod, 1.50 hit mod, 80 loot 
        let pl_pike = { Ident = "Pike"; DamageMod = 1.00; AccuracyMod = 1.50; SkillName = "Pike"; EnduranceCost = 40.0 }
        // short staff - short staff - 20 end cost, 2.00 damage mod, 0.75 hit mod, 80 loot
        let pl_shortstaff = { Ident = "Short Staff"; DamageMod = 2.00; AccuracyMod = 0.75; SkillName = "Short Staff"; EnduranceCost = 20.0 }
        // staff - staff - 40 end cost, 6.00 damage mod, 0.60 hit mod, 120 loot
        let pl_staff = { Ident = "Staff"; DamageMod = 6.00; AccuracyMod = 0.60; SkillName = "Staff"; EnduranceCost = 40.0 }
        
        let weaponroom = new Room(Map [], "Baerholt's Weapons", "Baerholt's Weapons", 
                                RoomContents.Shop (EquipmentList
                                    ([
                                        (20.0, WeaponSlot (pl_short_sword));
                                        (40.0, WeaponSlot (pl_scimitar));
                                        (60.0, WeaponSlot (pl_broadsword));
                                        (100.0, WeaponSlot (pl_greatsword));
                                        (50.0, WeaponSlot (pl_cudgel));
                                        (75.0, WeaponSlot (pl_hammer));
                                        (100.0, WeaponSlot (pl_ballandchain));
                                        (125.0, WeaponSlot (pl_warmattock));
                                        (30.0, WeaponSlot (pl_gauntlet));
                                        (30.0, WeaponSlot (pl_shortbow));
                                        (60.0, WeaponSlot (pl_longbow));
                                        (80.0, WeaponSlot (pl_compbow));
                                        (70.0, WeaponSlot (pl_lightxbow));
                                        (125.0, WeaponSlot (pl_heavyxbow));
                                        (10.0, WeaponSlot (pl_sling));
                                        (25.0, WeaponSlot (pl_staffsling));
                                        (80.0, WeaponSlot (pl_halberd));
                                        (80.0, WeaponSlot (pl_pike));
                                        (80.0, WeaponSlot (pl_shortstaff));
                                        (120.0, WeaponSlot (pl_staff))
                                    ])))

        eelroom.Neighbors <- Map [("East", unarmedgobroom)]
        unarmedgobroom.Neighbors <- Map [("West", eelroom); ("South", armedgobroom)]
        introom.Neighbors <- Map [("South", westgateroom); ("East", disroom)]
        armedgobroom.Neighbors <- Map [("North", unarmedgobroom); ("East", westgateroom)]
        westgateroom.Neighbors <- Map [("West", armedgobroom); ("North", introom); ("East", bridgeroom); ("South", charoom)]
        charoom.Neighbors <- Map [("North", westgateroom); ("East", agiroom)]
        disroom.Neighbors <- Map [("West", introom); ("South", bridgeroom); ("East", wisroom)]
        bridgeroom.Neighbors <- Map [("West", westgateroom); ("North", disroom); ("East", paladinroom); ("South", agiroom)]
        agiroom.Neighbors <- Map [("West", charoom); ("North", bridgeroom); ("East", barbroom); ("South", refroom)]
        refroom.Neighbors <- Map [("North", agiroom); ("East", staroom); ("South", armorroom)]
        armorroom.Neighbors <- Map [("North", refroom); ("East", strroom); ("South", weaponroom)]
        weaponroom.Neighbors <- Map [("North", armorroom); ("South", ratroom)]
        ratroom.Neighbors <- Map [("North", weaponroom)]
        strroom.Neighbors <- Map [("West", armorroom); ("North", staroom); ("East", snowbeastroom)]
        staroom.Neighbors <- Map [("West", refroom); ("North", barbroom); ("East", bloodwolfroom); ("South", strroom)]
        barbroom.Neighbors <- Map [("West", agiroom); ("North", paladinroom); ("East", reaverroom); ("South", staroom)]
        paladinroom.Neighbors <- Map [("West", bridgeroom); ("North", wisroom); ("East", eastgateroom); ("South", barbroom)]
        wisroom.Neighbors <- Map [("West", disroom); ("East", cougarroom); ("South", paladinroom)]
        cougarroom.Neighbors <- Map [("West", wisroom); ("East", woodtrollroom); ("South", eastgateroom)]
        eastgateroom.Neighbors <- Map [("West", paladinroom); ("North", cougarroom); ("South", reaverroom)]
        reaverroom.Neighbors <- Map [("West", barbroom); ("North", eastgateroom); ("East", beisswurmroom); ("South", bloodwolfroom)]
        bloodwolfroom.Neighbors <- Map [("West", staroom); ("North", reaverroom); ("East", rocktrollroom); ("South", snowbeastroom)]
        snowbeastroom.Neighbors <- Map [("West", strroom); ("North", bloodwolfroom)]

        let spr = [(ratroom, ratspawn); (unarmedgobroom, unarmedgobspawn); (armedgobroom, armedgobspawn); 
                   (cougarroom, cougarspawn); (woodtrollroom, woodtrollspawn); (eelroom, eelspawn); 
                   (reaverroom, reaverspawn); (beisswurmroom, beisswurmspawn); (bloodwolfroom, bloodwolfspawn); 
                   (rocktrollroom, rocktrollspawn); (snowbeastroom, snowbeastspawn)]

        let doMainTick (o : obj) = 
            spr
            |> List.iter (fun (r : Room, sp : MonsterSpawner) -> 
                            sp.CheckSituation ()
                            sp.TrySpawning ()

                            sp.BattleContext.Units
                            |> List.filter (fun (u : Unit) -> u.Vitals.Status = StatusType.Respawning)
                            |> List.iter (fun (u : Unit) -> 
                                            r.LeaveBattle u.Ident
                                            u.Vitals.Status <- Alive
                                            u.Vitals.Health <- sparams.RespawnHealthPct * u.Vitals.HealthMax
                                            u.Vitals.Endurance <- sparams.RespawnEndurancePct * u.Vitals.EnduranceMax
                                          )
                                          
                            r.Travelers
                            |> List.iter (fun (u : Unit) -> 
                                                u.AdjustHealth (sparams.HealthPctRecovery * u.Vitals.HealthMax)
                                                u.AdjustEndurance (sparams.EndurancePctRecovery * u.Vitals.EnduranceMax)))

        let doPulseTick (o : obj) = 
            spr
            |> List.iter (fun (r : Room, sp : MonsterSpawner) ->
                            r.Travelers
                            |> List.iter (fun (u : Unit) -> u.Pulse ())

                            sp.BattleContext.Units
                            |> List.iter (fun (u : Unit) -> u.Pulse ())
                         )

        let run () = 
            let o = new obj()

            let main_timer = new System.Threading.Timer((new TimerCallback(doMainTick)), o, 100, int (sparams.MainFreq.TotalMilliseconds))
            let pulse_timer = new System.Threading.Timer((new TimerCallback(doPulseTick)), o, 150, int (sparams.PulseFreq.TotalMilliseconds))

            while true do ()

