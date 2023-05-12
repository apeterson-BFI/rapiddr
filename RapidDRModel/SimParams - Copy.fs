namespace RapidDRModel
    open System
    open NodaTime

    type SimParams(pulseFreq : Duration, gainFreq : Duration, mainFreq : Duration) = 
        member this.PulseFreq = pulseFreq

        member this.GainFreq = gainFreq

        member this.MainFreq = mainFreq

        member this.BitsToRankReduction = 8.0       // changes to this not supported yet

        member this.MaximumSkill = 1750.0

        member this.MaximumStat = 150.0

        member this.TDPCostMult = 1.5               // changes to this not supported yet

        member this.CombatGainBitsMult = 1.0        // not implemented yet

        member this.HealthPctRecovery = 0.01

        member this.EndurancePctRecovery = 0.05

        member this.RespawnHealthPct = 0.2

        member this.RespawnEndurancePct = 0.0





