using LiteDB;
using Phony.Kernel;
using Phony.ViewModel;
using System;

namespace Phony.Model
{
    public class BaseModel : CommonBase
    {
        ViewModel.Users.LoginVM CurrentUser = new ViewModel.Users.LoginVM();

        public BaseModel()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
            }
            EditDate = DateTime.Now;
        }

        public long Id { get; set; }

        public string Notes { get; set; }

        [BsonRef(nameof(DBCollections.Users))]
        public virtual User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        [BsonRef(nameof(DBCollections.Users))]
        public virtual User Editor { get; set; }

        public DateTime? EditDate { get; set; }
    }
}