﻿// <copyright file="Notifier.cs" company="EnsageSharp">
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
namespace Ability.Core.AbilityFactory.Utilities
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     The notifier.
    /// </summary>
    public class Notifier : IDisposable
    {
        #region Public Properties

        public Collection<Action> Reacters { get; set; } = new Collection<Action>();

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.Reacters.Clear();
            this.Reacters = null;
        }

        public void Notify()
        {
            foreach (var reacter in this.Reacters)
            {
                reacter.Invoke();
            }
        }

        public virtual void Subscribe(Action reacter)
        {
            this.Reacters.Add(reacter);
        }

        #endregion
    }
}