// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkipType.cs" company="F. LEVEQUE">
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
// Based on work of SCOTT HANSELMAN (http://www.hanselman.com/blog/MixingXmlSerializersWithXElementsAndLINQToXML.aspx)
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Provide extension methods for xml serializer
    /// </summary>
    public static class XmlSerializerExtension
    {
        #region Methods

        /// <summary>
        /// Transform product of xml serializer in an XElement
        /// <remarks>Namespaces are striped from the XElement</remarks>
        /// </summary>
        /// <param name="xs">Xml serializer on which to perform tranformation</param>
        /// <param name="o">object to be serialized</param>
        /// <returns>XElement composed of the serialized object</returns>
        public static XElement SerializeAsXElement(this XmlSerializer xs, object o)
        {
            XDocument documentBuffer = new XDocument();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (XmlWriter writer = documentBuffer.CreateWriter())
            {
                xs.Serialize(writer, o, ns);
            }

            XElement result = documentBuffer.Root;
            result.Remove();

            return result;
        }

        #endregion Methods
    }
}