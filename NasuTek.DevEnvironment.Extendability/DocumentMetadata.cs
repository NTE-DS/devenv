using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public class DocumentMetadata
    {
        public string FilePath { get; set; }
        public Guid RequestedFormat { get; set; }
        public bool IsFile { get; set; }
        public List<object> Metadata { get; private set; }
        public object DataObject { get { return Metadata[0]; } }

        public DocumentMetadata() {
            Metadata = new List<object>();
            RequestedFormat = Guid.Parse("{E10C70C1-3507-45EC-99D2-151BA920D23A}"); // Default Text Editor TypeGUID
        }
    }
}
