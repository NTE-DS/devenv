using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment
{
    public class DocumentMetadata
    {
        public string FilePath { get; set; }
        public string RequestedFormat { get; set; }
        public bool IsFile { get; set; }
        public List<object> Metadata { get; private set; }
        public object DataObject { get { return Metadata[0]; } }

        public DocumentMetadata() {
            Metadata = new List<object>();
        }
    }
}
