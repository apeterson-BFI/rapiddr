namespace RapidDRModel

        open System
        open NodaTime

        type Attributes(stamina : float, strength : float, agility : float, reflex : float, charisma : float, discipline : float, intelligence : float, wisdom : float) = 
            member val Stamina = stamina

            member val Strength = strength

            member val Agility = agility

            member val Reflex = reflex

            member val Charisma = charisma

            member val Discipline = discipline

            member val Intelligence = intelligence

            member val Wisdom = wisdom

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