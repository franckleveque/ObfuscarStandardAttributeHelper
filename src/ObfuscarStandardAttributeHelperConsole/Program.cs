// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="F. LEVEQUE">
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
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
            ShowDisclaimer();
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
        /// Show disclaimer of application
        /// </summary>
        private static void ShowDisclaimer()
        {
            Console.WriteLine("ObfuscarStandardAttributeHelper Copyright (C) 2010 F. LEVEQUE");
            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY.");
            Console.WriteLine("This is free software, and you are welcome to redistribute it under certain conditions.");
            Console.WriteLine("See gpl.txt for more details.");
            Console.WriteLine();
        }

        /// <summary>
        /// Describe to the console how to use the application
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine("ObfuscarStandardAttributeHelper takes exactly one argument which is the obfuscar configuration file");
            Console.WriteLine(" ex : {0} obfuscar.xml", typeof(Program).Assembly.FullName);
        }

        #endregion Methods
    }
}