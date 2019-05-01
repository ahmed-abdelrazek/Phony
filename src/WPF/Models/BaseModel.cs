using LiteDB;
using System;

namespace Phony.WPF.Models
{
    public class BaseModel
    {
        public long Id { get; set; }

        public string Notes { get; set; }

        [BsonRef(nameof(Data.DBCollections.Users))]
        public virtual User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        [BsonRef(nameof(Data.DBCollections.Users))]
        public virtual User Editor { get; set; }

        public DateTime? EditDate { get; set; }
    }
}