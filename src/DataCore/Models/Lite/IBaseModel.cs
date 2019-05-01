using System;

namespace Phony.WPF.Models.Lite
{
    public interface IBaseModel
    {
        uint Id { get; set; }

        string Notes { get; set; }

        DateTime CreatedOn { get; set; }

        User Creator { get; set; }

        DateTime? EditedOn { get; set; }

        User Editor { get; set; }
    }
}