namespace RapidDRModel

    open System
    open NodaTime

    type StatusType =
        | Alive
        | Respawning
        | Annihilated

    type Vitals(health : float, endurance : float, concentration : float, respawns : float, status : StatusType) = 
        let mutable health = health
        
        member this.Health
            with get() = health
            and set(value) = 
                health <- value
                if health <= 0.0 then 
                    if this.Respawns <= 0.0 then this.Status <- Annihilated
                    else
                        this.Respawns <- this.Respawns - 1.0
                        this.Status <- Respawning
                else
                    this.Status <- Alive
        
        member val Endurance = endurance with get,set

        member val Concentration = concentration with get,set

        member val Respawns = respawns with get,set

        member val Status = status with get,set

