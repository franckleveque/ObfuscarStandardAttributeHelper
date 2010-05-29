namespace ObfuscarStandardAttributeHelper.Core.AssemblyScanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Class to manage assembly scanning
    /// </summary>
    public class AssemblyScanner
    {
        #region Fields

        /// <summary>
        /// Assembly to be scanned
        /// </summary>
        private Assembly ass;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssemblyScanner class.
        /// </summary>
        /// <param name="fileName">Path of assembly to scan</param>
        public AssemblyScanner(string fileName)
        {
            this.ass = Assembly.LoadFile(fileName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Scan the assembly
        /// </summary>
        /// <returns>Return the list of elements to be skipped</returns>
        public List<Configuration.SkipBase> ScanAssembly()
        {
            List<Configuration.SkipBase> result = new List<ObfuscarStandardAttributeHelper.Core.Configuration.SkipBase>();

            foreach (Type curType in this.ass.GetTypes())
            {
                result.AddRange(this.ScanType(curType, null));
            }

            return result;
        }

        /// <summary>
        /// Scan members of a type
        /// </summary>
        /// <param name="typeToScan">Type to scanned</param>
        /// <param name="defaultValue">Default behavior of elements (True if they have to be skipped)</param>
        /// <returns>List of skipped members</returns>
        private List<Configuration.SkipBase> ScanMembers(Type typeToScan, bool? defaultValue)
        {
            BindingFlags flags = 
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly;

            List<Configuration.SkipBase> result = new List<ObfuscarStandardAttributeHelper.Core.Configuration.SkipBase>();
            foreach (MethodInfo curMethod in typeToScan.GetMethods(flags))
            {
                // Only methods of class are to be excluded
                if (curMethod.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    foreach (Attribute curAtt in curMethod.GetCustomAttributes(typeof(System.Reflection.ObfuscationAttribute), false))
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        ObfuscationAttribute temp = curAtt as ObfuscationAttribute;
                        toExclude = temp.Exclude;
                    }

                    if (toExclude)
                    {
                        Configuration.SkipMethod toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipMethod();
                        toAdd.Type = typeToScan.FullName;
                        toAdd.Name = curMethod.Name;
                        result.Add(toAdd);
                    }
                }
            }

            foreach (PropertyInfo curProperty in typeToScan.GetProperties(flags))
            {
                // Only methods of class are to be excluded
                if (curProperty.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    foreach (Attribute curAtt in curProperty.GetCustomAttributes(typeof(System.Reflection.ObfuscationAttribute), false))
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        ObfuscationAttribute temp = curAtt as ObfuscationAttribute;
                        toExclude = temp.Exclude;
                    }

                    if (toExclude)
                    {
                        Configuration.SkipProperty toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipProperty();
                        toAdd.Type = typeToScan.FullName;
                        toAdd.Name = curProperty.Name;
                        result.Add(toAdd);
                    }
                }
            }

            foreach (FieldInfo curField in typeToScan.GetFields(flags))
            {
                // Only methods of class are to be excluded
                if (curField.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    foreach (Attribute curAtt in curField.GetCustomAttributes(typeof(System.Reflection.ObfuscationAttribute), false))
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        ObfuscationAttribute temp = curAtt as ObfuscationAttribute;
                        toExclude = temp.Exclude;
                    }

                    if (toExclude)
                    {
                        Configuration.SkipField toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipField();
                        toAdd.Type = typeToScan.FullName;
                        toAdd.Name = curField.Name;
                        result.Add(toAdd);
                    }
                }
            }

            foreach (EventInfo curEvent in typeToScan.GetEvents(flags))
            {
                // Only methods of class are to be excluded
                if (curEvent.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    foreach (Attribute curAtt in curEvent.GetCustomAttributes(typeof(System.Reflection.ObfuscationAttribute), false))
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        ObfuscationAttribute temp = curAtt as ObfuscationAttribute;
                        toExclude = temp.Exclude;
                    }

                    if (toExclude)
                    {
                        Configuration.SkipEvent toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipEvent();
                        toAdd.Type = typeToScan.FullName;
                        toAdd.Name = curEvent.Name;
                        result.Add(toAdd);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Scan a type of the assembly
        /// </summary>
        /// <param name="curType">Type to be scanned</param>
        /// <param name="defaultValue">Default behavior of elements (True if they have to be skipped)</param>
        /// <returns>List of skipped members</returns>
        private List<Configuration.SkipBase> ScanType(Type curType, bool? defaultValue)
        {
            List<Configuration.SkipBase> result = new List<ObfuscarStandardAttributeHelper.Core.Configuration.SkipBase>();
            Configuration.SkipType toAdd = null;
            foreach (Attribute curAtt in curType.GetCustomAttributes(typeof(System.Reflection.ObfuscationAttribute), false))
            {
                // We have found an Obfuscation attribute, default value doesn't apply
                ObfuscationAttribute temp = curAtt as ObfuscationAttribute;

                // Do we need to skip obsfuscation ?
                if (temp.ApplyToMembers == true)
                {
                    defaultValue = temp.Exclude;
                }
                else
                {
                    toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipType();
                    toAdd.Name = curType.FullName;
                }
            }

            List<Configuration.SkipBase> elems =
                this.ScanMembers(curType, defaultValue);

            if (defaultValue.HasValue && defaultValue.Value == true)
            {
                toAdd = new ObfuscarStandardAttributeHelper.Core.Configuration.SkipType();
                toAdd.Name = curType.FullName;
                /*
                if (elems.Count == 0)
                {
                    if (curType.GetFields().Count() > 0)
                    {
                        toAdd.SkipFields = true;
                    }

                    if (curType.GetEvents().Count() > 0)
                    {
                        toAdd.SkipEvents = true;
                    }

                    if (curType.GetProperties().Count() > 0)
                    {
                        toAdd.SkipProperties = true;
                    }

                    if (curType.GetMethods().Count() > 0)
                    {
                        toAdd.SkipMethods = true;
                    }
                }
                */
            }

            if (toAdd != null)
            {
                result.Add(toAdd);
            }

            result.AddRange(elems);

            foreach (Type subType in curType.GetNestedTypes())
            {
                result.AddRange(this.ScanType(subType, defaultValue));
            }

            return result;
        }

        #endregion Methods
    }
}