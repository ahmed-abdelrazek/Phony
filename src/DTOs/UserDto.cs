using Prism.Mvvm;

namespace Phony.DTOs
{
    public class UserDto : BindableBase
    {
        private int id;
        private string name;
        private string pass;
        private EnumDto group;
        private string phone;
        private string notes;
        private bool isActive;

        public int Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string Pass { get => pass; set => SetProperty(ref pass, value); }

        public EnumDto Group { get => group; set => SetProperty(ref group, value); }

        public string Phone { get => phone; set => SetProperty(ref phone, value); }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public bool IsActive { get => isActive; set => SetProperty(ref isActive, value); }
    }
}
