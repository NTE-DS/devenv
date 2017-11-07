using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using DumpWriter;

namespace ner71
{
    public class ExceptionHandle {
        private static string appName;
        private static Version appVersion;

        public static void AttachHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public static void SetAppName(string appName)
        {
            ExceptionHandle.appName = appName;
        }

        public static void SetAppVersion(Version appVersion)
        {
            ExceptionHandle.appVersion = appVersion;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Handle((Exception)e.ExceptionObject);
        }

        private static void Handle(Exception e) {
            var bucketPath = Path.GetTempFileName();
            File.Delete(bucketPath);
            Directory.CreateDirectory(bucketPath);

            var dump = new DumpWriter.DumpWriter(TextWriter.Null);
            dump.Dump(Process.GetCurrentProcess().Id, GetDumpType(), Path.Combine(bucketPath, "minidump.dmp"));

            SoapFormatter xmlSerializer = new SoapFormatter();
            using (var file = File.Create(Path.Combine(bucketPath, "Exception.xml"))) {
                xmlSerializer.Serialize(file, e);
            }

            var ini = new IniFile(Path.Combine(bucketPath, "NER_Manifest.ini"));
            ini.Write("Application Name", appName, "Manifest");
            ini.Write("Application Version", appVersion.ToString(), "Manifest");

            new Form1(bucketPath).ShowDialog();
            Directory.Delete(bucketPath, true);

            Process.GetCurrentProcess().Kill();
        }

        private static DumpType GetDumpType() {
            if (Properties.Settings.Default.Ner71_FullMiniDumps)
                return DumpType.FullMemory;

            return DumpType.MinimalWithFullCLRHeap;
        }
    }
}
