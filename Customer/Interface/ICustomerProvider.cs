using Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Providers
{
    public interface ICustomerProvider
    {
        List<CustomerWithOrder> GetSalesCustomerQuery(decimal minSumOrder, DateTime minDate, DateTime maxDate);
    }
}
