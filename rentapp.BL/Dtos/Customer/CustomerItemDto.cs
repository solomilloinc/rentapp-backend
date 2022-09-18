using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.Dtos.Customer
{
    public class CustomerItemDto
    {
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public int DocumentTypeId { get; set; }
        public string LastName { get; set; }
        public List<string> Addresses { get; set; }
    }
}
