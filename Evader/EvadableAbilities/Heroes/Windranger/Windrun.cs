﻿namespace Evader.EvadableAbilities.Heroes.Windranger
{
    using Base;
    using Base.Interfaces;

    using Ensage;

    using Modifiers;

    using static Data.AbilityNames;

    internal class Windrun : EvadableAbility, IModifier
    {
        #region Constructors and Destructors

        public Windrun(Ability ability)
            : base(ability)
        {
            Modifier = new EvadableModifier(EnemyTeam, EvadableModifier.GetHeroType.ModifierSource);

            Modifier.EnemyCounterAbilities.Add(Eul);
            Modifier.EnemyCounterAbilities.Add(FortunesEnd);
            Modifier.EnemyCounterAbilities.AddRange(Invul);
        }

        #endregion

        #region Public Properties

        public EvadableModifier Modifier { get; }

        #endregion

        #region Public Methods and Operators

        public override void Check()
        {
        }

        public override void Draw()
        {
        }

        public override float GetRemainingTime(Hero hero = null)
        {
            return 0;
        }

        #endregion
    }
}