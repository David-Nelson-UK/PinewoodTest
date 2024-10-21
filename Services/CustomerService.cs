using Microsoft.Extensions.Caching.Memory;
using Pinewood.Models;
using System.Collections.Generic;
using System.Linq;


namespace Pinewood.Services
{
    public interface ICustomerService
    {
        List<Customer> ReadAll();
        Customer Read(int id);
        void Create(Customer customer);
        void Update(Customer modifiedCustomer);
        void Delete(int id);
    }
    
    public class CustomerService : ICustomerService
    {
        private readonly IMemoryCache _cache;
        public CustomerService(IMemoryCache cache)
        {
            _cache = cache;
        }   
        public List<Customer> ReadAll()
        {
            if(_cache.Get("Customers") == null)
            {
                List<Customer> customers = new List<Customer>() {
                    new Customer { Id = 1, Name = "David Nelson", Email = "dave@david-nelson.uk",Address = "72 Friary Road,Atherstone,Warwickshire,CV93AL,UK",Phone="07725752924" },
                    new Customer { Id = 2, Name = "Elwood Blues", Email = "elwood@bluesbrothers.com",Address="1060 West Addison Street,Chicago,Illinois,60613,USA",Phone="634-5789"}
                };
                _cache.Set("Customers", customers);
            }
            return _cache.Get<List<Customer>>("Customers");
        }
        public void Create(Customer customer)
        {
            var customers = ReadAll();
            customer.Id = customers.Max(c => c.Id) + 1;
            customers.Add(customer);
            _cache.Set("Customers", customers);
        }
        public Customer Read(int id)
        {
            return ReadAll().Single(c=> c.Id == id);
        }
        public void Update(Customer modifiedCustomer)
        {
            var customers = ReadAll();
            var customer = customers.Single(c => c.Id == modifiedCustomer.Id);
            customer.Name = modifiedCustomer.Name;
            customer.Email = modifiedCustomer.Email;
            customer.Address = modifiedCustomer.Address;
            customer.Phone = modifiedCustomer.Phone;
            _cache.Set("Customers", customers);
        }
        public void Delete(int id)
        {
            var customers = ReadAll();
            var deletedCustomer = customers.Single(c => c.Id == id);
            customers.Remove(deletedCustomer);
            _cache.Set("Customers", customers);
        }
    }
}
