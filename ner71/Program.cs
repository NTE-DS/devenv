using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ner71
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            var bf = new BinaryFormatter();
            ExceptionBinder binder;

            using (var file = File.Open(args[0], FileMode.Open))
            {
                binder = (ExceptionBinder)bf.Deserialize(file);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(binder));
        }
    }
}
