using System;
using CustomerManagement.DataContext;
using CustomerManagement.Helper;
using CustomerManagement.Interfaces;
using CustomerManagement.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ProductManagement.Repositories
{
    public class CustomerRepository : ICustomerInterface
    {
        private readonly CustomerDBContext _context;

        public CustomerRepository(CustomerDBContext context)
        {
            _context = context;
        }

        public IEnumerable<CustomerModel> GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }

        public PagedList<CustomerModel> GetPaginatedCustomers(String? keywords, PagedListParams pagedListParams)
        {
            var filteredCustomers = _context.Customers.ToList();
            if (!String.IsNullOrEmpty(keywords))
            {
                filteredCustomers = _context.Customers.Where(d =>
                    d.Name.Contains(keywords) ||
                    d.Address.Contains(keywords)).ToList();
            }

            var customers = PagedList<CustomerModel>.ToPagedList(filteredCustomers,
                pagedListParams.PageNumber,
                pagedListParams.PageSize);

            return customers;
        }

        public CustomerModel GetCustomer(Int32 id)
        {
            var customer = _context.Customers.Where(d => d.Id == id).FirstOrDefault();
            return customer;
        }

        public Boolean CreateCustomer(CustomerModel customer)
        {
            _context.Add(customer);
            return Save();
        }

        public Boolean UpdateCustomer(CustomerModel customer)
        {
            _context.Update(customer);
            return Save();
        }

        public Boolean DeleteCustomer(CustomerModel customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public Boolean CustomerExists(Int32 id)
        {
            return _context.Customers.Any(o => o.Id == id);
        }

        public Boolean Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

