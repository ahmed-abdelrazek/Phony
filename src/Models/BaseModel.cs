using LiteDB;
using Phony.ViewModels;
using System;

namespace Phony.Models
{
    public class BaseModel
    {
        ViewModels.Users.LoginVM CurrentUser = new ViewModels.Users.LoginVM();

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