﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkipElement.cs" company="F. LEVEQUE">
//
// ObfuscarStandardAttributeHelper - Program used to make Obfuscar compatible with Standard obfuscation attributes
// Copyright (C) 2010 F. LEVEQUE
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Email: franck.leveque2@free.fr
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    using System.Xml.Serialization;

    /// <summary>
    /// Generics attributes of obfuscar skip xml elements
    /// </summary>
    public class SkipElement : SkipBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the attribute of object (Private, Public, ...)
        /// </summary>
        [XmlAttribute(AttributeName = "attrib")]
        public string Attribute
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of type from which object has to be skipped
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public string Type
        {
            get;
            set;
        }

        #endregion Properties
    }
}