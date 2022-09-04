using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class PictureProperty
    {
        public int PicturePropertyId { get; set; }
        public string PicturePath { get; set; } = null!;
        public int PopertyId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int DirectoryId { get; set; }

        public virtual Property Poperty { get; set; } = null!;
    }
}
