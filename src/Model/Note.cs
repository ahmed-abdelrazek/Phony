using Phony.ViewModel;

namespace Phony.Model
{
    public class Note : BaseModel
    {
        public string Name { get; set; }

        public NoteGroup Group { get; set; }

        public string Phone { get; set; }
    }
}