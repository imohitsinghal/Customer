using API.Data;
using API.Data.Repository;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Customer.Models;

namespace Customer.Providers
{

    public class CustomerProvider : ICustomerProvider
    {
        private AdventureWorks2019DbContext _dbContext;
        public CustomerProvider(AdventureWorks2019DbContext context)
        {
            _dbContext = context;
        }

        public Task<List<CustomerWithOrder>> GetSalesCustomerQuery(decimal minSumOrder, DateTime minDate, DateTime maxDate)
        {

            return (from order in _dbContext.SalesOrderHeaders
                    join customer in _dbContext.Customers on order.CustomerId equals customer.CustomerId
                    join person in _dbContext.People on customer.PersonId equals person.BusinessEntityId
                    where order.OrderDate <= maxDate && order.OrderDate > minDate
                    group new { order, person } by new
                    {
                        person.BusinessEntityId,
                        person.FirstName,
                        person.LastName,
                        person.MiddleName
                    } into result
                    where result.Sum(f => f.order.SubTotal) > minSumOrder
                    //from result in g
                    select new CustomerWithOrder
                    {
                        Id = result.Key.BusinessEntityId,
                        FirstName = result.Key.FirstName,
                        LastName = result.Key.LastName,
                        MiddleName = result.Key.MiddleName,
                        OrderDate = result.Max(o=> o.order.OrderDate),
                        OrderAmount = result.Sum(o => o.order.SubTotal),
                    }).ToListAsync();
        }

    }
}
