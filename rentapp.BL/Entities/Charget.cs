using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Charget
    {
        public int ChargetId { get; set; }
        public int ContractId { get; set; }
        public int CustomerPaymentId { get; set; }
        public DateTime ChargetDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int UserCreatedId { get; set; }
        public int ChargetStatusId { get; set; }
        public string? Observation { get; set; }
        public int DirectoryId { get; set; }

        public virtual Contract Contract { get; set; } = null!;
        public virtual CustomerPayment CustomerPayment { get; set; } = null!;
    }
}
