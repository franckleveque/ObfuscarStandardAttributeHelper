using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    public static class XmlSerializerExtension
    {
        public static XElement SerializeAsXElement(this XmlSerializer xs, object o)
        {
            XDocument d = new XDocument();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (XmlWriter w = d.CreateWriter()) xs.Serialize(w, o, ns);
            XElement e = d.Root;
            e.Remove();

            return e;
        }
    }
}
