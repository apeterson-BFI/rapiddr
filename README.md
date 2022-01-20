# rapiddr : Finding the Fun Strategy in Less Time
Classic Dragonrealms-style strategic choices rpg in less than 10 years of grinding.
Focussed on PvE advancement experience in a (first focus) solo or (future+++) small group setting

## Goals

1. Encapsulate the choices that guide player development over years of grind-time, and the trade-offs.
  1. How to focus initial stats and stat growth in very early through very late game. The benefits of Int, Wisdom and Discipline on the Learning Pool.
  2. How to develop ability trees, and the strategic benefit of different classes.
  3. Which / How many weapons to train, and where to focus. Melee, normal Ranged, Thrown, Brawling, Polearm et al
  4. Which / How many armors and other combat elements to train, dealing with expense and hinderance from carrying around gear, or time in going back to the vault.
  5. Play Style considerations: Stealthy, Defensive, Attacking.
  6. Choice of which monsters to hunt and where on the safety curve you choose.
  7. How many lores and other non-combat skills do you train?
  8. How much do you focus on skills in some skillsets over others. Powerset weight.
  9. Chosing Provinces / Areas, and how do you support backtraining when needed without massive travel time.
  10. Items from normal to special.
2. Provide a reasonably faithful DR experience with existing lessons (survival tert sucks, evasion is key, choose safer monsters mostly), but also provide a more balanced experience that will surprise people who have played DR a lot.
3. Update the experience for a more Idle / Rapid experience. You should be able to go through early Rat, Goblin, Cougar, Eel progression in 30 minutes. Allow for idle hunting when not online, adjust where you hunt and deal with loot, etc.
4. To support Idle experience, allow customizing time investment on different types of training.
5. Creating a tinkerable platform for exploring this kind of experience, by making it open source.

## Roadmap

1. Enable the barebones Barbarian hunting experience with Skill < 100 monsters to support it.
  1. Unit (character & monsters) statistics system  : simple start v1
    1. Statistics (slow changing attributes)
    2. Skills (faster changing attributes)
    3. Vitals (current endurance, current health, life/death, # of respawns (orbs))
    4. Level
    5. Equipped Items
    6. Stored Items
  2. Equipment System : simple start v1
    1. Body equip slots: Armor, Right Hand, Left Hand (non-weapon), container
    2. Containers: (v1 includes unlimited container space).
  3. Combat System : simple start v1
    1. Attacking for different types (melee, ranged, thrown)
    2. Benefits and drawbacks for different weapons in the same type
    3. Advantage system (manuevering into an advantage)
    4. Stance to determine weight of Evasion / Parry / Shield use
    5. Multi-Opponent combat
    6. Result tables for attack success
    7. Defenses: Evasion, Parry and Shield
    8. Damage Reduction for Effective Defense
    9. Armor - flat result table reduction and proportional protection, encumerance harming evasion's effectiveness.
    10. Determining death
  4. Map System : simple start v1
    1. Graph nodes and edges corresponding to compass directions and diagonals.
    2. Interfacing with other systems at map points supported.
  6. Monster System : simple start v1
    1. Summoning in zones based on player presence.
    2. Monster loot available
    3. Designing skills, stats and equipment for <100 monsters
  6. Loot System : simple start v1
    1. Provide indicated loot on kill with %s.
    2. Room for growth to full on loot system.
  7. Experience system : simple start v1
    1. Link attack and evade/parry/shield and armor with appropriate bits going into pools.
    2. Gradually move bits from pools into the appropriate skill, providing gain.
    3. Develop new Bits required to rank up formula.
    4. Mental stats provide benefits to help increase the rate of bits going from pool to skill gain.
    5. Combat ability provides increased ability to fill into the pool.
  8. Create Respawn System : simple start v1
    1. Spend time at Altar to gain respawns
    2. Lose undispersed experience on death.
    3. Room for future death penalties, skill loss, soul recovery, etc.
    4. Respawn wait time.
  9. Create Shop System : simple start v1
    1. Locations can be marked for buying equipment / selling loot.
    2. Locations can also be marked for increasing stats at the cost of TDP points.
    3. Locations can also be marked for increasing level, based on meeting Leveling structure (see next)
  10. Create Leveling System : simple start v1
    1. Barbarian Only for now
    2. Only combat skills required.
    3. Tiered requirements structure
    4. # ranks required in # skills of skillset.
    5. # ranks required in certain named skills.
