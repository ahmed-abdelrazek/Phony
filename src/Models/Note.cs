using Phony.Data;

namespace Phony.Models
{
    public class Note : BaseModel
    {
        public string Name { get; set; }

        public NoteGroup Group { get; set; }

        public string Phone { get; set; }
    }
}