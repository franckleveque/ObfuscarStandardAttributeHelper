namespace ObfuscarStandardAttributeHelper.ObfuscarStandardAttributeHelperConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using ObfuscarStandardAttributeHelper.Core.AssemblyScanning;
    using ObfuscarStandardAttributeHelper.Core.Configuration;

    /// <summary>
    /// Main class of the application
    /// </summary>
    public class Program
    {
        #region Methods

        /// <summary>
        /// Entry point of application
        /// </summary>
        /// <param name="args">
        /// Parameters of command line
        /// Takes only one parameter : path of xml obfuscar file 
        /// </param>
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.GetLength(0) != 1)
            {
                Program.ShowUsage();
                Environment.Exit(-1);
            }
            else
            {
                XDocument configFile = XDocument.Load(args[0]);
                string[] inPath = (from c in configFile.Element("Obfuscator").Elements()
                                   where c.Name.LocalName.Equals("Var") && c.Attribute("name").Value.Equals("InPath")
                                   select c.Attribute("value").Value).ToArray();
                List<XElement> assemblyList = configFile.Element("Obfuscator").Elements().ToList().FindAll(
                    delegate(XElement toCheck)
                    {
                        return toCheck.Name.LocalName.Equals("Module");
                    });

                foreach (XElement curModule in assemblyList)
                {
                    System.IO.FileInfo fileToLoad = new System.IO.FileInfo(curModule.Attribute("file").Value.Replace("$(InPath)", inPath[0]));
                    AssemblyScanner curScan = new AssemblyScanner(fileToLoad.FullName);
                    List<SkipBase> res = curScan.ScanAssembly();

                    foreach (SkipBase curSkip in res)
                    {
                        XmlSerializer xs = new XmlSerializer(curSkip.GetType());
                        XElement toAdd = xs.SerializeAsXElement(curSkip);
                        curModule.Add(toAdd);
                    }
                }

                configFile.Save(string.Format("{0}.headersAdded.xml", args[0]));
            }
        }

        /// <summary>
        /// Describe to the console how to use the application
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine("ObfuscarStandardAttributeHelper takes exactly one argument which is the obfuscar configuration file");
            Console.WriteLine(" ex : ObfuscarStandardAttributeHelper.exe obfuscar.xml");
        }

        #endregion Methods
    }
}