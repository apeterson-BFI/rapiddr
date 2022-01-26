namespace RapidDRModel

        open System
        open NodaTime

        type Attributes(stamina : float, strength : float, agility : float, reflex : float, charisma : float, discipline : float, intelligence : float, wisdom : float) = 
            member val Stamina = stamina with get,set

            member val Strength = strength with get,set

            member val Agility = agility with get,set

            member val Reflex = reflex with get,set

            member val Charisma = charisma with get,set

            member val Discipline = discipline with get,set

            member val Intelligence = intelligence with get,set

            member val Wisdom = wisdom with get,set

            member this.Find (statName : string) =
                if statName = "Stamina" then this.Stamina
                elif statName = "Strength" then this.Strength
                elif statName = "Agility" then this.Agility
                elif statName = "Reflex" then this.Reflex
                elif statName = "Charisma" then this.Charisma
                elif statName = "Discipline" then this.Discipline
                elif statName = "Intelligence" then this.Intelligence
                elif statName = "Wisdom" then this.Wisdom
                else failwith "Unknown attribute"

            member this.Set (statName : string) (amount : float) = 
                if statName = "Stamina" then this.Stamina <- amount
                elif statName = "Strength" then this.Strength <- amount
                elif statName = "Agility" then this.Agility <- amount
                elif statName = "Reflex" then this.Reflex <- amount
                elif statName = "Charisma" then this.Charisma <- amount
                elif statName = "Discipline" then this.Discipline <- amount
                elif statName = "Intelligence" then this.Intelligence <- amount
                elif statName = "Wisdom" then this.Wisdom <- amount
                else failwith "Unknown attribute"

            member this.Increase (availTDPs: float) (statName : string) = 
                let currStat = this.Find statName

                if availTDPs >= this.IncreaseCost currStat 
                then this.Set statName (currStat + 1.0)
                else ()
                
            member private this.IncreaseCost (stat : float) = 3.0 * stat 

            member this.TDPCost(start : Attributes) = 
                1.5 * (this.Stamina * this.Stamina + this.Strength * this.Strength + this.Agility * this.Agility + this.Reflex * this.Reflex +
                       this.Charisma * this.Charisma + this.Discipline * this.Discipline + 
                       this.Intelligence * this.Intelligence + this.Wisdom * this.Wisdom + 
                       this.Stamina + this.Strength + this.Agility + this.Reflex + 
                       this.Charisma + this.Discipline + this.Intelligence + this.Wisdom) -
                1.5 * (start.Stamina * start.Stamina + start.Strength * start.Strength + start.Agility * start.Agility + start.Reflex * start.Reflex +
                       start.Charisma * start.Charisma + start.Discipline * start.Discipline + 
                       start.Intelligence * start.Intelligence + start.Wisdom * start.Wisdom + 
                       start.Stamina + start.Strength + start.Agility + start.Reflex + 
                       start.Charisma + start.Discipline + start.Intelligence + start.Wisdom)

            new(attr : Attributes) = 
                Attributes(attr.Stamina, attr.Strength, attr.Agility, attr.Reflex, attr.Charisma, attr.Discipline, attr.Intelligence, attr.Wisdom)