using System;

namespace Phony.Data.Models.Lite
{
    public interface IBaseModel
    {
        long Id { get; set; }

        string Notes { get; set; }

        DateTime CreatedAt { get; set; }

        User Creator { get; set; }

        DateTime EditedAt { get; set; }

        User Editor { get; set; }
    }
}