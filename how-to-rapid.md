# How to take the DR systems and make them play out more rapidly.

Purely brainstorming Ideas:

1. Reduce the bits required to achieve each rank.
2. Increase the size of pools and amount of bits pulsing (eff. same as 1)
3. Make common actions automated and actually execute the same actions faster in the sim [speed up the sim itself].

We don't want to send attack commands a jillion times. In fact, attacks should be auto (at least in first versions).
Either we need to be able to store enough loot to go sell hundreds of monster parts, or each piece of loot should be scale up.

Categorizing:

Model Scaling reduces the number of actions required to reach end game, by lowering requirements for each part.
Speed Scaling maintains the number of actions required but accelerates how fast they are taken.

Model Scaling would probably be more fun and still interesting.
Speed Scaling would be more consistent in exactly scaling the stuff that needs to be done now to get skills to 2000.

Speed scaling will stress the server more.
Model Scaling will require carely look at hard caps by monster type and preventing excess overflow.

Choice:

For now, do Model Scaling. We want to capture the strategic decisions, not the actual or simulated tedium.


Types of Model Scaling:

From Fresh - design a new set of numbers of monster bits gained, pool dynamics, and bits to rank et all that,

From Speed-inspired - using model changes to reflect an abstraction of Speed Scaling.


Speed Inspired will help keep the character of the DR choices and tradeoffs.

Choice: Speed Inspired.


## Current base DR Mechanics (news to me I had OOOLD formulas)

Maximum Skill: 1750

total bits accumulated at rank : bits = rank (rank + 399) / 2
rank = -399 / 2 + sqrt(399^2 + 8 bit) / 2

base pool size :
primary 15000 x / (x + 900) + 1000
seconary 12750 x / (x + 900) + 850
tertiary 10500 x / (x + 900) + 700

Int value 	Formula
< 30 	y=((x-10)*60)/10
30-60 	y=(((x-30)*30)+1200)/10
>60 	y=(((x-60)*15)+2100)/10 

Disc value 	Formula
< 30 	y=((x-10)*20)/10
30-60 	y=(((x-30)*10)+400)/10
>60 	y=(((x-60)*5)+700)/10 

Total Base Pool: y=((1000+int_val+disc_val)/1000)* vase pool 

Skillset 	Time to Pulse From Mind Lock to Clear
Primary 	40-60 minutes
Secondary 	50-80 minutes
Tertiary 	70-100 minutes 

 The primary factor affecting this fraction is whether the skill is primary, secondary, or tertiary; higher Wisdom also increases this fraction
 
 compared to 10
 Intelligence/Wisdom 	30 	112%
Intelligence/Wisdom 	60 	121%
Intelligence/Wisdom 	90 	125%
Intelligence/Wisdom 	120 	130% 


## Questions

How do bits increase in higher hunting grounds.

Initially look at the scaling to increase bits proportional to increases in pool size. It should take some time and effort to fill primary, tertiary should be kinda easy

At 0 - 1000 bit primary, 850 bit secondary, 700 bit tertiary
At 900 - 8500 bit primary, 7225 bit secondary, 5950 secondary

This will take tuning to get it right

## Accelerated Model

Provisional Plan

Accelerate time to pulse from Mind Lock to Clear by 60.
Accelerate time to fill pool by 60.
Reduce bits to rank by 8 fold 

provisional Guess play time to reach maxes: 10 years * 300 days / year * 6 hours / day = 18000 hours or 750 days of play time.

Resulting in an 480-1 accelerating: 10 years @ 300/6 becomes 37.5 hours.
Seems reasonable. 37.5 hours is still a lot of time to experience the arc, once everything is created.

Adjustabke PULSE / DRAIN time.
Adjustable BIT reqd
Will help when there's only enough content for a blip of time at 480 scale.
