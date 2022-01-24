namespace RapidDRModel
    open System
    open NodaTime

    type SimParams(pulseFreq : Duration, gainFreq : Duration) = 
        member this.PulseFreq = pulseFreq

        member this.GainFreq = gainFreq

        member this.BitsToRankReduction = 8.0       // changes to this not supported yet

        member this.MaximumSkill = 1750.0

        member this.MaximumStat = 150.0

        member this.TDPCostMult = 1.5               // changes to this not supported yet

        member this.CombatGainBitsMult = 1.0        // not implemented yet



