using DataCore.Data;
using System;

namespace Phony.WPF.Models.Lite
{
    public class Item : IBaseModel
    {
        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Shopcode { get; set; }

        public byte[] Image { get; set; }

        public ItemGroup Group { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal WholeSalePrice { get; set; }

        public decimal HalfWholeSalePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal QTY { get; set; }

        public Company Company { get; set; }

        public virtual Supplier Supplier { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}