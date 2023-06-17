using System;
using CustomerManagement.Helper;
using CustomerManagement.Models;

namespace CustomerManagement.Interfaces
{
    public interface ICustomerInterface
    {
        public IEnumerable<CustomerModel> GetCustomers();
        public PagedList<CustomerModel> GetPaginatedCustomers(String? keywords, PagedListParams pagedListParams);
        public CustomerModel GetCustomer(Int32 id);
        public Boolean CreateCustomer(CustomerModel customer);
        public Boolean UpdateCustomer(CustomerModel customer);
        public Boolean DeleteCustomer(CustomerModel customer);
        public Boolean CustomerExists(Int32 id);
    }
}