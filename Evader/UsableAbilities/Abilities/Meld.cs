namespace Evader.UsableAbilities.Abilities
{
    using System;
    using System.Linq;

    using Base;

    using Core;
    using Core.Menus;

    using Data;

    using Ensage;
    using Ensage.Common.Objects.UtilityObjects;

    using EvadableAbilities.Base;

    using AbilityType = Data.AbilityType;

    internal class Meld : NotTargetable, IDisposable
    {
        #region Fields

        private readonly Sleeper sleeper;

        #endregion

        #region Constructors and Destructors

        public Meld(Ability ability, AbilityType type, AbilityCastTarget target = AbilityCastTarget.Self)
            : base(ability, type, target)
        {
            sleeper = new Sleeper();
            Player.OnExecuteOrder += PlayerOnExecuteOrder;
        }

        #endregion

        #region Properties

        private static UsableAbilitiesMenu Menu => Variables.Menu.UsableAbilities;

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            Player.OnExecuteOrder -= PlayerOnExecuteOrder;
        }

        public override void Use(EvadableAbility ability, Unit target)
        {
            base.Use(ability, target);
            if (Menu.MeldBlock)
            {
                sleeper.Sleep(Menu.MeldBlockTime);
            }
        }

        #endregion

        #region Methods

        private void PlayerOnExecuteOrder(Player sender, ExecuteOrderEventArgs args)
        {
            if (!sleeper.Sleeping || !args.Entities.Contains(Hero))
            {
                return;
            }

            switch (args.OrderId)
            {
                case OrderId.AttackLocation:
                case OrderId.AttackTarget:
                case OrderId.Stop:
                case OrderId.Hold:
                case OrderId.MoveTarget:
                case OrderId.MoveLocation:
                    args.Process = false;
                    break;
                case OrderId.AbilityTarget:
                case OrderId.AbilityLocation:
                case OrderId.Ability:
                    sleeper.Sleep(0);
                    break;
            }
        }

        #endregion
    }
}