using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Entities
{
    public class DirectoryCustomer
    {
        public int DirectoryCustomerId { get; set; }
        public int CustomerId { get; set; }
        public int DirectoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
    }
}
