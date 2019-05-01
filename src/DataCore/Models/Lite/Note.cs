using DataCore.Data;
using System;

namespace Phony.WPF.Models.Lite
{
    public class Note : IBaseModel
    {
        public string Name { get; set; }

        public NoteGroup Group { get; set; }

        public string Phone { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}