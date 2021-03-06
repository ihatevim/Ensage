﻿namespace Evader.EvadableAbilities.Heroes.Silencer
{
    using Base;
    using Base.Interfaces;

    using Ensage;

    using Modifiers;

    using static Data.AbilityNames;

    internal class GlobalSilence : NoObstacleAbility, IModifier
    {
        #region Constructors and Destructors

        public GlobalSilence(Ability ability)
            : base(ability)
        {
            Modifier = new EvadableModifier(HeroTeam, EvadableModifier.GetHeroType.LowestHealth);

            BlinkAbilities.Clear();

            Modifier.AllyCounterAbilities.Add(Lotus);
            Modifier.AllyCounterAbilities.Add(Eul);
            Modifier.AllyCounterAbilities.Add(Manta);
            Modifier.AllyCounterAbilities.AddRange(AllyPurges);
            Modifier.AllyCounterAbilities.AddRange(AllyShields);
        }

        #endregion

        #region Public Properties

        public EvadableModifier Modifier { get; }

        #endregion
    }
}