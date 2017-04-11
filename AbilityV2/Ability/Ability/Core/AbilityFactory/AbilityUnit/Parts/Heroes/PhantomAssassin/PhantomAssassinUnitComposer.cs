﻿// <copyright file="PhantomAssassinUnitComposer.cs" company="EnsageSharp">
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
namespace Ability.Core.AbilityFactory.AbilityUnit.Parts.Heroes.PhantomAssassin
{
    using System.ComponentModel.Composition;

    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.Composer;
    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.PositionTracker;
    using Ability.Core.AbilityFactory.AbilityUnit.Parts.Default.PositionTracker.Types;
    using Ability.Core.AbilityFactory.Metadata;

    using Ensage;

    /// <summary>The shadow demon unit composer.</summary>
    [Export(typeof(IAbilityUnitComposer))]
    [AbilityUnitMetadata(ClassId.CDOTA_Unit_Hero_PhantomAssassin)]
    internal class PhantomAssassinUnitComposer : AbilityUnitComposer
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="PhantomAssassinUnitComposer" /> class.</summary>
        public PhantomAssassinUnitComposer()
        {
            this.AssignPart<IPositionTracker>(unit => new DotaBasePositionTracker(unit, 450));
        }

        #endregion
    }
}