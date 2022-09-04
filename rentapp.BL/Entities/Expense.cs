using System;
using System.Collections.Generic;

namespace rentapp.BL.Entities
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string? Observation { get; set; }
        public int? ExpenseTypeId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public int DirectoryId { get; set; }

        public virtual Directory Directory { get; set; } = null!;
    }
}
