using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NasuTek.DevEnvironment.Extensibility;
using System.Reflection;
using NasuTek.DevEnvironment.Project;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public class ProjectService {
        public static readonly object[] EmptyObjects = new object[] {};

        private static Dictionary<Guid, IProjectGenerator> m_ProjectGenerators = new Dictionary<Guid, IProjectGenerator>();
        private static Dictionary<string, Tuple<Guid, string, string[]>> m_ProjExtensions = new Dictionary<string, Tuple<Guid, string, string[]>>();

        static ProjectService() {
            var regSvc = (IDevEnvRegSvc)DevEnvSvc.GetService(DevEnvSvc.RegSvc);

            if (regSvc.SubKeyExists(SettingsReg.Global, "ProjectTypes"))
                foreach (var addin in regSvc.OpenSubKey(SettingsReg.Global, "ProjectTypes").GetSubKeys())
                {
                    var guid = Guid.Parse(addin.Name);
                    var name = (string)addin.GetValue(null);

                    var addInAssemb = AppDomain.CurrentDomain.GetAssemblies().First(v => v.GetName().FullName == (string)addin.GetValue("ProjectAssembly"));

                    var addinPlug = (IProjectGenerator)addInAssemb.CreateInstance((string)addin.GetValue("ProjectClass"));
                    m_ProjectGenerators.Add(guid, addinPlug);

                    foreach (var ext in ((string)addin.GetValue("ProjectExtension")).Split(';').Where(ext => !m_ProjExtensions.ContainsKey(ext)))
                    {
                        m_ProjExtensions.Add(ext, Tuple.Create(guid, name, new string[] { ext }.Concat(addin.GetValue("CompatableExtensions") != null ? ((string)addin.GetValue("CompatableExtensions")).Split(';') : new string[] { }).ToArray()));
                    }
                }
        }

        public static ISolution OpenSolution(string solutionFilePath) {
            throw new NotImplementedException();
        }

        public static ISolution CreateTempSolutionFromProject(IProject proj)
        {
            var sol = new DefaultSolution();
            sol.SolutionName = proj.ProjectName;
            sol.AddProject(proj);
            return sol;
        }

        public static IProject OpenProject(string projectFilePath) {
            return OpenProject(projectFilePath, m_ProjExtensions[Path.GetExtension(projectFilePath)].Item1);
        }

        public static IProject OpenProject(string projectFilePath, Guid generator) {
            return m_ProjectGenerators[generator].Open(projectFilePath);
        }

        public static string SaveFilterString(string ext) {
            if (ext == null) return "";

            string outStr = m_ProjExtensions.Where(projExtension => projExtension.Value.Item3.Contains(ext) && projExtension.Key != ext)
                .Aggregate(m_ProjExtensions[ext].Item2 + " (*" + ext + ")|*" + ext + "|", (current, projExtension) => current + (projExtension.Value.Item2 + " (*" + projExtension.Key + ")|*" + projExtension.Key + "|"));
            return outStr.Substring(0, outStr.Length - 1);
        }
        public static string FilterString {
            get {
                var outStr = m_ProjExtensions.Aggregate("", (current, projExtension) => current + (projExtension.Value.Item2 + " (*" + projExtension.Key + ")|*" + projExtension.Key + "|"));
                return outStr.Length > 0 ? outStr.Substring(0, outStr.Length - 1) : "";
            }
        }
    }
}
