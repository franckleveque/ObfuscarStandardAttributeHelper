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
            XDocument d = new XDocument();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);
            using (XmlWriter w = d.CreateWriter())
            {
                xs.Serialize(w, o, ns);
            }

            XElement e = d.Root;
            e.Remove();

            return e;
        }

        #endregion Methods
    }
}