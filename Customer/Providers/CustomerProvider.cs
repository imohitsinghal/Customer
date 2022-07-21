using API.Data;
using API.Data.Repository;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Customer.Providers
{
    
    public class CustomerProvider: ICustomerProvider
    {
        private AdventureWorks2019DbContext _dbContext;
        public CustomerProvider(AdventureWorks2019DbContext context)
        {
            _dbContext = context;
        }

        public Task<List<API.Data.Repository.Models.Customer>> GetSalesCustomerQuery(decimal minSumOrder,DateTime minDate, DateTime maxDate) 
        {
            return (from order in _dbContext.SalesOrderHeaders
                    join customer in _dbContext.Customers on order.CustomerId equals customer.CustomerId
                    where order.SubTotal >= minSumOrder && order.OrderDate <= maxDate && order.OrderDate > minDate
                    select customer).ToListAsync();
        }
        
    }
}
