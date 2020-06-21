using Phony.Data.Core;
using System;

namespace Phony.Data.Models.Lite
{
    public class Note : BaseModel
    {
        public string Name { get; set; }

        public NoteGroup Group { get; set; }

        public string Phone { get; set; }
    }
}