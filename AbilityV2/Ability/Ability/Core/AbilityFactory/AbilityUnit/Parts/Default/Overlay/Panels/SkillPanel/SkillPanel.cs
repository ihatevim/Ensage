﻿// <copyright file="SkillPanel.cs" company="EnsageSharp">
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
namespace Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Overlay.Panels.SkillPanel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ability.Core.AbilityFactory.AbilitySkill;
    using Ability.Core.AbilityFactory.AbilitySkill.Parts.DefaultParts.SkillLevel;
    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Overlay.Panels.ObjectPanel;
    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.SkillBook;
    using Ability.Core.AbilityFactory.Utilities;

    using SharpDX;

    /// <summary>
    ///     The skill panel.
    /// </summary>
    public class SkillPanel : ObjectPanel<SkillPanelObject>, ISkillPanel
    {
        #region Fields

        private DataObserver<ISkillLevel> levelObserver;

        private Func<IAbilitySkill, bool> selectionCondition;

        private DataObserver<SkillAdd> skillAddObserver;

        private DataObserver<SkillRemove> skillRemoveObserver;

        /// <summary>The temp dictionary.</summary>
        private Dictionary<double, SkillPanelObject> tempDictionary = new Dictionary<double, SkillPanelObject>();

        #endregion

        #region Constructors and Destructors

        public SkillPanel(
            IAbilityUnit unit,
            PanelDirection direction,
            Vector2 defaultObjectSize,
            Func<IAbilitySkill, bool> selectionCondition,
            Func<SkillPanelObject, uint> orderFunction = null,
            Func<SkillPanelObject, Vector2, Vector2> objectSizeFunction = null)
            : base(unit,
                direction,
                defaultObjectSize,
                orderFunction,
                objectSizeFunction ?? ((o, vector2) =>
                    {
                        if (o.Skill.Level.Current <= 0)
                        {
                            return vector2 * new Vector2(0.5f);
                        }
                        else if (o.Skill.Cooldown == null && o.Skill.Charges == null)
                        {
                            return vector2 * new Vector2(0.65f);
                        }

                        return vector2;
                    }))
        {
            this.Unit = unit;
            this.levelObserver = new DataObserver<ISkillLevel>(level => { this.ObjectManager.UpdateSize(); });
            this.SelectionCondition = selectionCondition;

            // Console.WriteLine(this.tempDictionary.Count);
            // foreach (var keyValuePair in this.Unit.SkillBook.AllSkills)
            // {
            // if (this.SelectionCondition.Invoke(keyValuePair.Value))
            // {
            // var o = new SkillPanelObject(keyValuePair.Value);
            // Console.WriteLine(keyValuePair.Value.Name);
            // this.tempDictionary.Add(keyValuePair.Key, o);
            // this.AddObject(o);
            // this.levelObserver.Subscribe(keyValuePair.Value.Level);
            // }
            // }
            this.skillAddObserver = new DataObserver<SkillAdd>(
                add =>
                    {
                        if (this.SelectionCondition.Invoke(add.Skill))
                        {
                            var o = new SkillPanelObject(add.Skill);
                            this.tempDictionary.Add(add.Skill.SkillHandle, o);
                            this.AddObject(o);
                        }

                        this.levelObserver.Subscribe(add.Skill.Level);
                    });
            this.skillAddObserver.Subscribe(this.Unit.SkillBook.SkillAdd);

            this.skillRemoveObserver = new DataObserver<SkillRemove>(
                remove =>
                    {
                        if (!this.SelectionCondition.Invoke(remove.Skill))
                        {
                            return;
                        }

                        this.RemoveObject(this.TempDictionary[remove.Skill.SkillHandle]);
                    });

            this.skillRemoveObserver.Subscribe(unit.SkillBook.SkillRemove);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the default health bar position.
        /// </summary>
        public override int DefaultHealthBarPosition { get; } = (int)PanelDirection.Top;

        /// <summary>Gets the description.</summary>
        public override string Description { get; } = "Generates and draws SkillOverlay of unit's spells";

        /// <summary>
        ///     Gets a value indicating whether generate menu.
        /// </summary>
        public override bool GenerateMenuBool { get; } = true;

        /// <summary>
        ///     Gets the menu display name.
        /// </summary>
        public override string MenuDisplayName { get; } = "SpellPanel";

        /// <summary>
        ///     Gets the menu name.
        /// </summary>
        public override string MenuName { get; } = nameof(SkillPanel);

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the order function.
        /// </summary>
        public Func<IAbilitySkill, uint> OrderFunction { get; set; }

        /// <summary>
        ///     Gets or sets the position from health bar.
        /// </summary>
        public Func<Vector2> PositionFromHealthBarFunc { get; set; }

        /// <summary>
        ///     Gets or sets the selection condition.
        /// </summary>
        public Func<IAbilitySkill, bool> SelectionCondition
        {
            get
            {
                return this.selectionCondition;
            }

            set
            {
                this.selectionCondition = value;
                this.UpdateSkills();
            }
        }

        /// <summary>The temp dictionary.</summary>
        public Dictionary<double, SkillPanelObject> TempDictionary
        {
            get
            {
                return this.tempDictionary;
            }

            set
            {
                this.tempDictionary = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>The dispose.</summary>
        public override void Dispose()
        {
            base.Dispose();

            this.tempDictionary.Clear();
            this.levelObserver.Dispose();
            this.skillAddObserver.Dispose();
            this.skillRemoveObserver.Dispose();
        }

        /// <summary>
        ///     The update skills.
        /// </summary>
        public void UpdateSkills()
        {
            var newskills = new List<SkillPanelObject>();
            foreach (var keyValuePair in this.Unit.SkillBook.AllSkills)
            {
                if (!this.selectionCondition.Invoke(keyValuePair.Value))
                {
                    continue;
                }

                SkillPanelObject o;
                if (!this.tempDictionary.TryGetValue(keyValuePair.Key, out o))
                {
                    o = new SkillPanelObject(keyValuePair.Value);
                    this.tempDictionary.Add(keyValuePair.Key, o);
                    this.levelObserver.Subscribe(keyValuePair.Value.Level);
                }

                // this.tempDictionary.Add(keyValuePair.Key, o);
                newskills.Add(o);
            }

            if (newskills.Count == 0 || this.ObjectManager.OrderFunction == null)
            {
                this.ObjectManager.Objects = newskills;
                this.Panel?.UpdateSize();
                return;
            }

            this.ObjectManager.Objects = newskills.OrderByDescending(this.ObjectManager.OrderFunction).ToList();
            this.Panel?.UpdateSize();
        }

        #endregion
    }
}