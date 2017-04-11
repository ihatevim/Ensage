﻿// <copyright file="HealthBar.cs" company="EnsageSharp">
//    Copyright (c) 2017 Moones.
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see http://www.gnu.org/licenses/
// </copyright>
namespace Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Overlay.Bars
{
    using System;
    using System.Collections.Generic;

    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Health;
    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Overlay.PanelFields;
    using Ability.Core.AbilityFactory.Utilities;
    using Ability.Core.MenuManager.Menus.Submenus.UnitMenu;

    using Ensage;
    using Ensage.Common.Menu;
    using Ensage.Common.Menu.Transitions;
    using Ensage.Common.Objects.DrawObjects;

    using SharpDX;

    /// <summary>
    ///     The health bar.
    /// </summary>
    public class HealthBar : DrawObject, IBar
    {
        #region Fields

        /// <summary>
        ///     The health lost transition.
        /// </summary>
        private readonly Transition healthLostTransition = new QuadEaseOut(0.5);

        /// <summary>
        ///     The bg pos.
        /// </summary>
        private Vector2 barPos;

        /// <summary>
        ///     The bg size.
        /// </summary>
        private Vector2 barSize;

        /// <summary>
        ///     The fill.
        /// </summary>
        private float fill;

        /// <summary>
        ///     The fill size.
        /// </summary>
        private Vector2 fillSize;

        /// <summary>
        ///     The health lost size.
        /// </summary>
        private Vector2 healthLostSize;

        private DataObserver<IHealth> healthObserver;

        private float lastSeparatedValue;

        /// <summary>The lines.</summary>
        private Dictionary<float, HealthSeparator> lines = new Dictionary<float, HealthSeparator>();

        private Action maxHealthObserver;

        /// <summary>
        ///     The pos.
        /// </summary>
        private Vector2 pos;

        /// <summary>
        ///     The size.
        /// </summary>
        private Vector2 size;

        #endregion

        #region Constructors and Destructors

        public HealthBar(IAbilityUnit unit, Vector2 size)
        {
            this.Unit = unit;
            this.healthObserver = new DataObserver<IHealth>(this.UpdateHealth);
            this.maxHealthObserver = this.UpdateSeparators;
            this.healthObserver.Subscribe(this.Unit.Health);
            this.Size = size;
            this.lastSeparatedValue = 0;
            this.UpdateSeparators();
            this.Unit.Health.MaximumHealthChange.Subscribe(this.maxHealthObserver);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        ///     Gets or sets the color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets or sets the fill percentage.
        /// </summary>
        public float FillPercentage
        {
            get
            {
                return this.fill;
            }

            set
            {
                if (this.fill > value)
                {
                    this.healthLostSize = new Vector2(this.barSize.X / 100 * (this.fill - value), this.barSize.Y);
                    this.healthLostTransition.Start(0, 255);
                }

                this.fill = value;
                this.fillSize = new Vector2(this.barSize.X / 100 * this.fill, this.barSize.Y);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether generate menu.
        /// </summary>
        public bool GenerateMenuBool { get; }

        /// <summary>
        ///     Gets the parent element.
        /// </summary>
        public PanelField Panel { get; set; }

        /// <summary>
        ///     Gets or sets the parent element.
        /// </summary>
        public IUnitOverlayElement ParentElement { get; set; }

        /// <summary>
        ///     Gets or sets the position.
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return this.pos;
            }

            set
            {
                this.pos = value;
                this.barPos = this.pos + new Vector2(this.size.X / 40);
                foreach (var healthSeparator in this.lines)
                {
                    healthSeparator.Value.HealthBarPositionChange();
                }
            }
        }

        /// <summary>
        ///     Gets the position from health bar.
        /// </summary>
        public Vector2 PositionFromHealthBar => new Vector2(0);

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        public override sealed Vector2 Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
                this.barSize = this.size - new Vector2(this.size.X / 20);
                this.fillSize = new Vector2(this.barSize.X / 100 * this.fill, this.barSize.Y);
            }
        }

        /// <summary>
        ///     Gets or sets the size increase.
        /// </summary>
        public float SizeIncrease { get; set; }

        /// <summary>
        ///     Gets or sets the unit.
        /// </summary>
        public IAbilityUnit Unit { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The add submenu.
        /// </summary>
        /// <param name="menu">
        ///     The menu.
        /// </param>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu AddSubmenu(IUnitMenu menu)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The connect to menu.
        /// </summary>
        /// <param name="menu">
        ///     The menu.
        /// </param>
        public void ConnectToMenu(IUnitMenu menu, Menu subMenu)
        {
            // this.LeftPanel.ConnectToMenu(menu, subMenu);
            // this.TopPanel.ConnectToMenu(menu, subMenu);
            // this.RightPanel.ConnectToMenu(menu, subMenu);
        }

        /// <summary>The dispose.</summary>
        public void Dispose()
        {
            this.healthObserver.Dispose();
            this.Unit.Health.MaximumHealthChange.Reacters.Remove(this.maxHealthObserver);
        }

        /// <summary>
        ///     The draw.
        /// </summary>
        public override void Draw()
        {
            Drawing.DrawRect(this.Position, this.Size, this.BackgroundColor);
            Drawing.DrawRect(this.barPos, this.fillSize, this.Color);

            if (this.healthLostTransition.Moving)
            {
                var color = new Color(255, 255, 255, 255 - (int)this.healthLostTransition.GetValue());
                Drawing.DrawRect(this.barPos + new Vector2(this.fillSize.X, 0), this.healthLostSize, color);
            }

            foreach (var healthSeparator in this.lines)
            {
                if (!healthSeparator.Value.Visible)
                {
                    return;
                }

                healthSeparator.Value.Draw();
            }
        }

        /// <summary>
        ///     The generate menu.
        /// </summary>
        /// <param name="menu">
        ///     The menu.
        /// </param>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu GenerateMenu(IUnitMenu menu)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The generate menu.
        /// </summary>
        /// <returns>
        ///     The <see cref="Menu" />.
        /// </returns>
        public Menu GenerateMenu()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The set position.
        /// </summary>
        /// <param name="healthbarPosition">
        ///     The healthbar position.
        /// </param>
        public void SetPosition(Vector2 healthbarPosition)
        {
            this.Position = healthbarPosition;
        }

        public void UpdateHealth(IHealth value)
        {
            this.FillPercentage = value.Percentage;

            foreach (var healthSeparator in this.lines)
            {
                healthSeparator.Value.HealthChange();
            }
        }

        #endregion

        #region Methods

        /// <summary>The update separators.</summary>
        private void UpdateSeparators()
        {
            foreach (var healthSeparator in this.lines)
            {
                healthSeparator.Value.MaxHealthChange();
            }

            if (this.lastSeparatedValue > this.Unit.Health.Maximum)
            {
                var count = 0f;
                for (var i = this.lastSeparatedValue; i > this.Unit.Health.Maximum; i -= 500)
                {
                    this.lines.Remove(i);
                    this.lastSeparatedValue = i;
                    count++;
                }

                this.lastSeparatedValue -= 500;
            }
            else if (this.lastSeparatedValue + 500 < this.Unit.Health.Maximum)
            {
                var count = 0f;
                this.lastSeparatedValue += 500;
                for (var i = this.lastSeparatedValue; i < this.Unit.Health.Maximum; i += 500)
                {
                    this.lines.Add(i, new HealthSeparator(i, this));
                    this.lastSeparatedValue = i;
                    count++;
                }
            }
        }

        #endregion
    }
}