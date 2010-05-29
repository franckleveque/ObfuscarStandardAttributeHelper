namespace ObfuscarStandardAttributeHelper.Core.Configuration
{
    using System.Xml.Serialization;

    /// <summary>
    /// Base of Obfuscar skip xml elements
    /// </summary>
    public class SkipBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of object to be skipped
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name
        {
            get;
            set;
        }

        #endregion Properties
    }
}