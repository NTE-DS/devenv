using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NasuTek.DevEnvironment.Extensibility.Addins;

namespace NasuTek.DevEnvironment.Extensibility.Project
{
    public class ProjectService {
        public static readonly object[] EmptyObjects = new object[] {};

        private static Dictionary<string, IProjectGenerator> m_ProjectGenerators = new Dictionary<string, IProjectGenerator>();
        private static Dictionary<string, Tuple<string, Guid, string, string[]>> m_ProjExtensions = new Dictionary<string, Tuple<string, Guid, string, string[]>>();

        static ProjectService() {
            if (!AddInTree.ExistsTreeNode("/DevEnv/ProjectGenerators")) return;

            foreach (var i in AddInTree.GetTreeNode("/DevEnv/ProjectGenerators").Codons) {
                m_ProjectGenerators.Add(i.Id, (IProjectGenerator) i.AddIn.CreateObject(i.Properties["class"]));
                foreach (var ext in i.Properties["projectfileextension"].Split(';').Where(ext => !m_ProjExtensions.ContainsKey(ext))) {
                    m_ProjExtensions.Add(ext, Tuple.Create(i.Id, Guid.Parse(i.Properties["guid"]), i.Properties["projectTitle"], new string[] { i.Properties["projectfileextension"] }.Concat(i.Properties["compatextensions"].Split(';')).ToArray()));
                }
            }
        }

        public static ISolution OpenSolution(string solutionFilePath) {
            throw new NotImplementedException();
        }

        public static IProject OpenProject(string projectFilePath) {
            return OpenProject(projectFilePath, m_ProjExtensions[Path.GetExtension(projectFilePath)].Item1);
        }

        public static IProject OpenProject(string projectFilePath, string generator) {
            return m_ProjectGenerators[generator].Open(projectFilePath);
        }

        public static string SaveFilterString(string ext) {
            if (ext == null) return "";

            string outStr = m_ProjExtensions.Where(projExtension => projExtension.Value.Item4.Contains(ext) && projExtension.Key != ext)
                .Aggregate(m_ProjExtensions[ext].Item3 + " (*" + ext + ")|*" + ext + "|", (current, projExtension) => current + (projExtension.Value.Item3 + " (*" + projExtension.Key + ")|*" + projExtension.Key + "|"));
            return outStr.Substring(0, outStr.Length - 1);
        }
        public static string FilterString {
            get {
                var outStr = m_ProjExtensions.Aggregate("", (current, projExtension) => current + (projExtension.Value.Item3 + " (*" + projExtension.Key + ")|*" + projExtension.Key + "|"));
                return outStr.Length > 0 ? outStr.Substring(0, outStr.Length - 1) : "";
            }
        }
    }
}
