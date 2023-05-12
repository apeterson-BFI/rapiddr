# rapiddr : Finding the Fun Strategy in Less Time
Classic Dragonrealms-style strategic choices rpg in less than 10 years of grinding.
Focussed on PvE advancement experience in a (first focus) solo or (future+++) small group setting

## Goals

1. Encapsulate the choices that guide player development over years of grind-time, and the trade-offs.
    - How to focus initial stats and stat growth in very early through very late game. The benefits of Int, Wisdom and Discipline on the Learning Pool.
    - How to develop ability trees, and the strategic benefit of different classes.
    - Which / How many weapons to train, and where to focus. Melee, normal Ranged, Thrown, Brawling, Polearm et al
    - Which / How many armors and other combat elements to train, dealing with expense and hinderance from carrying around gear, or time in going back to the vault.
    - Play Style considerations: Stealthy, Defensive, Attacking.
    - Choice of which monsters to hunt and where on the safety curve you choose.
    - How many lores and other non-combat skills do you train?
    - How much do you focus on skills in some skillsets over others. Powerset weight.
    - Chosing Provinces / Areas, and how do you support backtraining when needed without massive travel time.
    - Items from normal to special.
2. Provide a reasonably faithful DR experience with existing lessons (survival tert sucks, evasion is key, choose safer monsters mostly), but also provide a more balanced experience that will surprise people who have played DR a lot.
3. Update the experience for a more Idle / Rapid experience. You should be able to go through early Rat, Goblin, Cougar, Eel progression in 30 minutes. Allow for idle hunting when not online, adjust where you hunt and deal with loot, etc.
4. To support Idle experience, allow customizing time investment on different types of training.
5. Creating a tinkerable platform for exploring this kind of experience, by making it open source.

## Roadmap

Setup architectural elements and program basics for the server.

1. SimEngine
    - Top level system
    - Control of Key simulation parameters
3. Model
    - Representation of every type DR data
    - Representation of Global game parameters
4. Persistence
    - Storage and saving Model state in case of server crash
6. AgentModel
    - Representation of the Agent and it's capabilities.
8. AgentView
    - Representation of information to be sent to client.
10. DebugView
    - Representation of information to be sent when debugging the Model's big picture view.

Setup interface between client (network wise, and special data protocol to send

1. Find / Build text-based mud networking server API.
2. Implement specific command logic to handle equipment, movement, shop interaction.
3. Implement specific Output presentation through AgentView.

use telnet, or modern networking ideas.

Setup client interface with useful information.

GUI interface designed around making textual information organization usefully.
1. Text display area
2. Text input area
3. Helpful information panels / displays.
4. Login, Logout
5. Character interface (start new : play existing)

MODEL: Enable the barebones Barbarian hunting experience with Skill < 100 monsters to support it.
  1. Unit (character & monsters) statistics system  : simple start v1
      - Statistics (slow changing attributes)
      - Skills (faster changing attributes)
      - Vitals (current endurance, current health, life/death, # of respawns (orbs))
      - Level
      - Equipped Items
      - Stored Items
  2. Equipment System : simple start v1
      - Body equip slots: Armor, Right Hand, Left Hand (non-weapon), container
      - Containers: (v1 includes unlimited container space).
  3. Combat System : simple start v1
      - Attacking for different types (melee, ranged, thrown)
      - Benefits and drawbacks for different weapons in the same type
      - Advantage system (manuevering into an advantage)
      - Stance to determine weight of Evasion / Parry / Shield use
      - Multi-Opponent combat
      - Result tables for attack success
      - Defenses: Evasion, Parry and Shield
      - Damage Reduction for Effective Defense
      - Armor - flat result table reduction and proportional protection, encumerance harming evasion's effectiveness.
      - Determining death
  4. Map System : simple start v1
      - Graph nodes and edges corresponding to compass directions and diagonals.
      - Interfacing with other systems at map points supported.
  6. Monster System : simple start v1
      - Summoning in zones based on player presence.
      - Monster loot available
      - Designing skills, stats and equipment for <100 monsters
  6. Loot System : simple start v1
      - Provide indicated loot on kill with %s.
      - Room for growth to full on loot system.
  7. Experience system : simple start v1
      - Link attack and evade/parry/shield and armor with appropriate bits going into pools.
      - Gradually move bits from pools into the appropriate skill, providing gain.
      - Develop new Bits required to rank up formula.
      - Mental stats provide benefits to help increase the rate of bits going from pool to skill gain.
      - Combat ability provides increased ability to fill into the pool.
  8. Create Respawn System : simple start v1
      - Spend time at Altar to gain respawns
      - Lose undispersed experience on death.
      - Room for future death penalties, skill loss, soul recovery, etc.
      - Respawn wait time.
  9. Create Shop System : simple start v1
      - Locations can be marked for buying equipment / selling loot.
      - Locations can also be marked for increasing stats at the cost of TDP points.
      - Locations can also be marked for increasing level, based on meeting Leveling structure (see next)
  10. Create Leveling System : simple start v1
      - Barbarian Only for now
      - Only combat skills required.
      - Tiered requirements structure
      - ranks required in # skills of skillset.
      - ranks required in certain named skills.
