using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    public class SkipBase
    {
        [XmlAttribute(AttributeName="name")]
        public string Name
        {
            get;
            set;
        }
    }
}
