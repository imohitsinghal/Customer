using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Providers
{
    public interface ICustomerProvider
    {
        Task<List<API.Data.Repository.Models.Customer>> GetSalesCustomerQuery(decimal minSumOrder, DateTime minDate, DateTime maxDate);
    }
}
