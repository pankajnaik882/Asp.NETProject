using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class NoteArchiveModel
    {
        public int NoteID { get; set; }

        public bool IsArchive { get; set; }

        public bool IsTrash { get; set; }

        public bool IsPin { get; set; }
    }
}
