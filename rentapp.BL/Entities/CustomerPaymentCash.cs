using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class CustomerPaymentCash
    {
        public int CustomerPaymentCashId { get; set; }
        public int? CustomerPaymentId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool? IsActive { get; set; }
        public int DirectoryId { get; set; }

        public virtual CustomerPayment? CustomerPayment { get; set; }
    }
}
