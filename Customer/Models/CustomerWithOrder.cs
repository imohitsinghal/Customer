using System;

namespace Customer.Models
{
    public class CustomerWithOrder
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderAmount { get; set; }
        public int ProductCategoryId { get; set; }

    }
}
