using System;
using Phony.Kernel;

namespace Phony.Model
{
    public class BaseModel : CommonBase
    {

        ViewModel.Users.LoginVM CurrentUser = new ViewModel.Users.LoginVM();

        public BaseModel()
        {
            EditDate = DateTime.Now;
            EditById = CurrentUser.Id;
        }

        public long Id { get; set; }

        public string Notes { get; set; }

        public int CreatedById { get; set; }

        public virtual User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public int? EditById { get; set; }

        public virtual User Editor { get; set; }

        public DateTime? EditDate { get; set; }
    }
}