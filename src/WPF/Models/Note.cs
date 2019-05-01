using Phony.WPF.Data;

namespace Phony.WPF.Models
{
    public class Note : BaseModel
    {
        public string Name { get; set; }

        public NoteGroup Group { get; set; }

        public string Phone { get; set; }
    }
}