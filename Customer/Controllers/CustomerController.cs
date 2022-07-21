using Customer.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerProvider _customerProvider;


        public CustomerController(ILogger<CustomerController> logger, ICustomerProvider customerProvider)
        {
            _logger = logger;
            _customerProvider = customerProvider;
        }

        [HttpGet("{minSumOrder}/{minDate}/{maxDate}")]
        public async Task<IEnumerable<API.Data.Repository.Models.Customer>> GetSalesCustomerQuery(decimal minSumOrder, DateTime minDate, DateTime maxDate)
        {
            return await _customerProvider.GetSalesCustomerQuery(minSumOrder, minDate, maxDate);
        }
    }
}
