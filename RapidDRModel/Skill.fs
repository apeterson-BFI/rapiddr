namespace RapidDRModel

    open System
    open NodaTime

    type SkillsetPoolParameters = { poolconst : float; poolincr : float; pooldenom : float; pulsebase : float; pulseincr : float }

    type Skill(name : string, skillbits : float, skillsetType : SkillsetPoolParameters, poolbits : float) = 

        let calcBasePool (s : SkillsetPoolParameters) (rank : float) =
            s.poolconst + s.poolincr * rank / (rank + s.pooldenom)

        let calcIntVal (intel : float) = 
            if intel < 30 then (intel - 10.0) * 6.0
            elif intel <= 60 then ((intel - 30.0) * 30.0 + 1200.0) / 10.0
            else ((intel - 60.0) * 15.0 + 2100.0) / 10.0

        let calcDiscVal (disc : float) =
            if disc < 30 then (disc - 10.0) * 2.0
            elif disc <= 60 then ((disc - 30.0) * 10.0 + 400.0) / 10.0
            else ((disc - 60.0) * 5.0 + 700.0) / 10.0

        let calcFinalPool (s : SkillsetPoolParameters) (intel : float) (disc : float) (rank : float) =
            let bp = calcBasePool s rank
            let iv = calcIntVal intel
            let dv = calcDiscVal disc
            (1000.0 + iv + dv) / 1000.0 * bp

        let calcRankFromBits (bits : float) = 
            -399.0 / 2.0 + Math.Sqrt(399.0 * 399.0 / 4.0 + 16.0 * bits)

        let calcBitsFromRank (rank : float) = 
            rank * (rank + 399.0) / 16.0

        let calcPulseRate (s : SkillsetPoolParameters) (wisdom : float) =
            let w10 = Math.Log10(wisdom)
            s.pulsebase + w10 * s.pulseincr

        let calcPulseBits (s : SkillsetPoolParameters) (wisdom : float) (intel : float) (disc : float) (rank : float) = 
            let cfp = calcFinalPool s intel disc rank
            let pr = calcPulseRate s wisdom
            pr * cfp
            
        let calcDesiredGainRate (rank : float) (s : SkillsetPoolParameters) (gainFreq : Duration) (pulseFreq : Duration) = 
            let expStats = 13.942 + 0.055 * rank
            let expPulseBits = calcPulseBits s expStats expStats expStats rank
            let expPoolBits = calcFinalPool s expStats expStats rank
            let goalPulses = 20.0 + rank / 1750.0 * 40.0

            // we want goalPulses to achieve filling the pool against the expPulseBits rate.
            // GAIN * gainsPerPulse * goal - expPulseBits * goal = expPoolBits
            // GAIN * gainsPerPulse - expPulseBits = expPoolBits / goal
            // GAIN = (expPoolBits / goal + expPulseBits) / gainsPerPulse
            let gainsPerPulse = gainFreq / pulseFreq

            (expPoolBits / goalPulses + expPulseBits) / gainsPerPulse

        let calcTDPAccum (rank : float) = rank * (rank + 1.0) / 400.0

        let calcEffectiveBitGain (rank : float) (bitsGained : float) (softCap : float) (hardCap : float) = 
            let ratio = if rank >= hardCap then 0.0 elif rank >= softCap then 0.5 - 0.5 * (rank - softCap) * (hardCap - softCap) else 1.0
            ratio * bitsGained

        member this.Name = name

        member this.SkillsetType = skillsetType

        member val SkillBits = skillbits with get, set

        member val PoolBits = poolbits with get, set

        member this.SkillRanks = 
            calcRankFromBits this.SkillBits

        member this.TDPSEarned = 
            calcTDPAccum this.SkillRanks

        member this.SetRank (rank : float) =
            this.SkillBits <- calcBitsFromRank rank

        member this.Pulse (wisdom : float) (intel : float) (disc : float) =
            let bp = calcPulseBits this.SkillsetType wisdom intel disc this.SkillRanks
            
            if bp <= this.PoolBits 
            then 
                this.SkillBits <- this.SkillBits + bp
                this.PoolBits <- this.PoolBits - bp
            else
                this.SkillBits <- this.SkillBits + this.PoolBits
                this.PoolBits <- 0.0

        member this.Gain (intel : float) (disc : float) (softCapRank : float) (hardCapRank : float) (gainBits : float) = 
            let gp = calcEffectiveBitGain this.SkillRanks gainBits softCapRank hardCapRank
            let maxpool = calcFinalPool this.SkillsetType intel disc this.SkillRanks

            this.PoolBits <- Math.Min(maxpool, this.PoolBits + gp)

        member this.DesiredGainRate (gainFreq : Duration) (pulseFreq : Duration) = 
            calcDesiredGainRate this.SkillRanks this.SkillsetType gainFreq pulseFreq

        new(skill : Skill) = Skill(skill.Name, 0.0, skill.SkillsetType, 0.0) 

        static member Primary = { poolconst = 1000.0; poolincr = 15000.0; pooldenom = 900.0; pulsebase = 0.025; pulseincr = 0.025 }
        static member Secondary = { poolconst = 850.0; poolincr = 12750.0; pooldenom = 900.0; pulsebase = 0.015; pulseincr = 0.0225 }
        static member Tertiary = { poolconst = 700.0; poolincr = 10500.0; pooldenom = 900.0; pulsebase = 0.013; pulseincr = 0.017 }        
