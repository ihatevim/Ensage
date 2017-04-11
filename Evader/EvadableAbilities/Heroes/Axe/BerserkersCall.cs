﻿namespace Evader.EvadableAbilities.Heroes.Axe
{
    using Base;
    using Base.Interfaces;

    using Ensage;

    using Modifiers;

    using static Data.AbilityNames;

    internal class BerserkersCall : AOE, IModifier
    {
        #region Constructors and Destructors

        public BerserkersCall(Ability ability)
            : base(ability)
        {
            Modifier = new EvadableModifier(HeroTeam, EvadableModifier.GetHeroType.LowestHealth);

            CounterAbilities.Add(PhaseShift);
            CounterAbilities.Add(BallLightning);
            CounterAbilities.Add(SleightOfFist);
            CounterAbilities.Add(Eul);
            CounterAbilities.Add(Manta);
            CounterAbilities.AddRange(VsDisable);
            CounterAbilities.AddRange(VsDamage);
            CounterAbilities.AddRange(VsPhys);
            CounterAbilities.Add(SnowBall);
            CounterAbilities.Add(Armlet);
            CounterAbilities.Add(Bloodstone);

            Modifier.AllyCounterAbilities.AddRange(AllyShields);
            Modifier.AllyCounterAbilities.AddRange(Invul);
            Modifier.AllyCounterAbilities.AddRange(VsPhys);
            Modifier.AllyCounterAbilities.Remove("item_glimmer_cape");
        }

        #endregion

        #region Public Properties

        public EvadableModifier Modifier { get; }

        #endregion
    }
}