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