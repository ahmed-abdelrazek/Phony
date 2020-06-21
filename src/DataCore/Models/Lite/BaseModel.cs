using System;

namespace Phony.Data.Models.Lite
{
    public class BaseModel : IBaseModel
    {
        private DateTime createdAt;
        private DateTime editedAt;

        public BaseModel()
        {
            EditedAt = DateTime.Now;
        }

        public long Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get => createdAt.ToLocalTime(); set => createdAt = value.ToUniversalTime(); }

        public virtual User Creator { get; set; }

        public DateTime EditedAt { get => editedAt.ToLocalTime(); set => editedAt = value.ToUniversalTime(); }

        public virtual User Editor { get; set; }
    }
}
