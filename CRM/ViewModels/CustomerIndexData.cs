using System.Collections.Generic;
using CRM.Models;

namespace CRM.ViewModels
{
    public class CustomerIndexData
    {
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Call> Calls { get; set; }
    }
}