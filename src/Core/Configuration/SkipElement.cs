using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    public class SkipElement : SkipBase
    {
        [XmlAttribute(AttributeName="type")]
        public string Type
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName="attrib")]
        public string Attribute
        {
            get;
            set;
        }
    }
}
