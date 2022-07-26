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

        public List<CustomerWithOrder> GetSalesCustomerQuery(decimal minSumOrder, DateTime minDate, DateTime maxDate)
        {
            var query = (from order in _dbContext.SalesOrderHeaders
                         join customer in _dbContext.Customers on order.CustomerId equals customer.CustomerId
                         join person in _dbContext.People on customer.PersonId equals person.BusinessEntityId
                         where order.OrderDate <= maxDate && order.OrderDate >= minDate
                         group new { order, person } by new
                         {
                             person.BusinessEntityId,
                             person.FirstName,
                             person.LastName,
                             person.MiddleName,
                             order.SalesOrderId
                         } into result



                         where result.Sum(f => f.order.SubTotal) > minSumOrder
                         select new
                         {
                             Id = result.Key.BusinessEntityId,
                             FirstName = result.Key.FirstName,
                             LastName = result.Key.LastName,
                             MiddleName = result.Key.MiddleName,
                             OrderDate = result.Max(o => o.order.OrderDate),
                             OrderAmount = result.Sum(o => o.order.SubTotal),
                             result.Key.SalesOrderId
                         } into data
                         join sod in _dbContext.SalesOrderDetails on data.SalesOrderId equals sod.SalesOrderId
                         join p in _dbContext.Products on sod.ProductId equals p.ProductId
                         join psc in _dbContext.ProductSubcategories on p.ProductSubcategoryId equals psc.ProductSubcategoryId
                         join pc in _dbContext.ProductCategories on psc.ProductCategoryId equals pc.ProductCategoryId
                         group new { data, pc } by new
                         {
                             data.Id,
                             data.FirstName,
                             data.LastName,
                             data.MiddleName,
                             data.SalesOrderId,
                             data.OrderAmount,
                             data.OrderDate
                         } into final
                         select new CustomerWithOrder
                         {
                             Id = final.Key.Id,
                             FirstName = final.Key.FirstName,
                             LastName = final.Key.LastName,
                             MiddleName = final.Key.MiddleName,
                             OrderDate = final.Key.OrderDate,
                             OrderAmount = final.Key.OrderAmount,
                             ProductCategoryId = final.Select(s => s.pc.ProductCategoryId).FirstOrDefault()
                         });

            var count = query.Distinct().ToList().Count;
            return query.ToList();
        }

    }
}
