namespace ObfuscarStandardAttributeHelper.Core.AssemblyScanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    // using System.Reflection;
    using System.Text;

    using Mono.Cecil;

    /// <summary>
    /// Class to manage assembly scanning
    /// </summary>
    public class AssemblyScanner
    {
        #region Fields

        /// <summary>
        /// Assembly to be scanned
        /// </summary>
        //private Assembly ass;
        private AssemblyDefinition ass;

        private string filePath;

        private bool modified = false;

        private string obfuscationType = typeof(System.Reflection.ObfuscationAttribute).FullName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssemblyScanner class.
        /// </summary>
        /// <param name="fileName">Path of assembly to scan</param>
        public AssemblyScanner(string fileName)
        {
            // this.ass = Assembly.LoadFile(fileName);
            this.ass = AssemblyDefinition.ReadAssembly(fileName);
            this.filePath = fileName;
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
            bool? toExclude = null;

            System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(this.ass);
            if (toFind != null)
            {
                // We have found an Obfuscation attribute, default value doesn't apply
                toExclude = toFind.Exclude;
            }

            if (!(toExclude.HasValue && toExclude.Value))
            {
                foreach (ModuleDefinition module in this.ass.Modules)
                {
                    foreach (TypeDefinition type in module.Types)
                    {
                        result.AddRange(this.ScanType(type, null));
                    }

                }
            }

            if (this.modified)
            {
                this.ass.Write(this.filePath);
            }

            return result;
        }

        /// <summary>
        /// Retrieve the obfuscation attribute and hide Cecil mecanism
        /// </summary>
        /// <param name="propertiesHolder">Object which has the custom attributes</param>
        /// <returns>A copy of the obfuscation attribute</returns>
        private System.Reflection.ObfuscationAttribute GetAndStripObfuscationCustomAttribute(ICustomAttributeProvider propertiesHolder)
        {
            System.Reflection.ObfuscationAttribute result = null;
            foreach (CustomAttribute curAtt in propertiesHolder.CustomAttributes)
            {
                if (curAtt.Constructor.DeclaringType.FullName.Equals(this.obfuscationType))
                {
                    // We have found an Obfuscation attribute, default value doesn't apply
                    // let's fill it's attributes
                    result = new System.Reflection.ObfuscationAttribute();
                    result.Feature = "all";
                    foreach (CustomAttributeNamedArgument curArg in curAtt.Properties)
                    {
                        switch (curArg.Name)
                        {
                            case "ApplyToMembers":
                                result.ApplyToMembers = (bool)curArg.Argument.Value;
                                break;
                            case "Exclude":
                                result.Exclude = (bool)curArg.Argument.Value;
                                break;
                            case "Feature":
                                result.Feature = (string)curArg.Argument.Value;
                                break;
                            case "StripAfterObfuscation":
                                result.StripAfterObfuscation = (bool)curArg.Argument.Value;
                                break;
                        }
                    }

                    // Let's do the job
                    if (result.StripAfterObfuscation)
                    {
                        propertiesHolder.CustomAttributes.Remove(curAtt);
                        this.modified = true;
                    }

                    // We have found what we are looking for, loop might have been modified, let's quit
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Scan members of a type
        /// </summary>
        /// <param name="typeToScan">Type to scanned</param>
        /// <param name="defaultValue">Default behavior of elements (True if they have to be skipped)</param>
        /// <returns>List of skipped members</returns>
        private List<Configuration.SkipBase> ScanMembers(TypeDefinition typeToScan, bool? defaultValue)
        {
            List<Configuration.SkipBase> result = new List<ObfuscarStandardAttributeHelper.Core.Configuration.SkipBase>();
            foreach (MethodDefinition curMethod in typeToScan.Methods)
            {
                // Only methods of class are to be excluded
                if (curMethod.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(curMethod);
                    if (toFind != null)
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        toExclude = toFind.Exclude;
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

            foreach (PropertyDefinition curProperty in typeToScan.Properties)
            {
                // Only methods of class are to be excluded
                if (curProperty.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(curProperty);
                    if (toFind != null)
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        toExclude = toFind.Exclude;
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

            foreach (FieldDefinition curField in typeToScan.Fields)
            {
                // Only methods of class are to be excluded
                if (curField.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(curField);
                    if (toFind != null)
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        toExclude = toFind.Exclude;
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

            foreach (EventDefinition curEvent in typeToScan.Events)
            {
                // Only methods of class are to be excluded
                if (curEvent.DeclaringType.FullName.Equals(typeToScan.FullName))
                {
                    bool toExclude = defaultValue.GetValueOrDefault(false);
                    System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(curEvent);
                    if (toFind != null)
                    {
                        // We have found an Obfuscation attribute, default value doesn't apply
                        toExclude = toFind.Exclude;
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
        private List<Configuration.SkipBase> ScanType(TypeDefinition curType, bool? defaultValue)
        {
            List<Configuration.SkipBase> result = new List<ObfuscarStandardAttributeHelper.Core.Configuration.SkipBase>();
            Configuration.SkipType toAdd = null;
            System.Reflection.ObfuscationAttribute toFind = this.GetAndStripObfuscationCustomAttribute(curType);
            if (toFind != null)
            {
                // Do we need to skip obsfuscation ?
                if (toFind.ApplyToMembers == true)
                {
                    defaultValue = toFind.Exclude;
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
            }

            if (toAdd != null)
            {
                result.Add(toAdd);
            }

            result.AddRange(elems);

            foreach (TypeDefinition subType in curType.NestedTypes)
            {
                result.AddRange(this.ScanType(subType, defaultValue));
            }

            return result;
        }

        #endregion Methods
    }
}