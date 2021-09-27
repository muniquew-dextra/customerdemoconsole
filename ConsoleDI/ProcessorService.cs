using ConsoleDI.Data;
using ConsoleDI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDI
{
    public class ProcessorService 
    {
        private readonly ILogger<ProcessorService> _log;
        private readonly IConfiguration _config;
        private readonly IBaseRepository _baseRepository;

        public ProcessorService(ILogger<ProcessorService> log, IConfiguration config, IBaseRepository baseRepository)
        {
            _log = log;
            _config = config;
            _baseRepository = baseRepository;
        }


        public async Task Run()
        {
            string keyVaultUrl = _config.GetValue<string>("KeyVaultUrl");
            string clientId = _config.GetValue<string>("ClientId");
            string tenantId = _config.GetValue<string>("TenantId");
            string clientSecret = _config.GetValue<string>("ClientSecret");

            string connectionString = await KeyVaultHandler.GetSecretValue("CustomerDemoConnection", keyVaultUrl, tenantId, clientId, clientSecret);

            Customer customer = ReadCustomerData();
            bool customerExists = await CheckExistingCustomer(connectionString, customer.Name);

            if(!customerExists)
            {
                bool success = await InsertCustomer(connectionString, customer);

                if(success)
                {
                    Console.WriteLine("Added!");
                }
            }
        }

        private Customer ReadCustomerData()
        {
            Customer customer = new Customer();

            Console.WriteLine("Enter name: ");
            customer.Name = Console.ReadLine();
            Console.WriteLine("Enter address: ");
            customer.Address = Console.ReadLine();
            Console.WriteLine("Enter phone: ");
            customer.Phone = Console.ReadLine();
            Console.WriteLine("Enter customer country: ");
            customer.Country = Console.ReadLine();

            return customer;
        }

        private async Task<bool> InsertCustomer(string connectionString, Customer customer)
        {
            try
            {
                return await _baseRepository.Execute(connectionString,
                    "INSERT INTO Customer(Name, Address, Phone, Country) VALUES (@Name, @Address, @Phone, @Country)", customer);
            }
            catch (DataRepositoryException ex)
            {
                _log.LogError($"Exception at InsertCustomer: {ex.Message} - StackTrace is: { ex }", ex.Message, ex);
                return false;
            }
        }

        private async Task<bool> CheckExistingCustomer(string connectionString, string name)
        {
            var customer = await GetCustomer(connectionString, name);

            if (customer != null)
            {
                return true;
            }

            return false;
        }

        private async Task<Customer> GetCustomer(string connectionString, string name)
        {
            try
            {
                return await _baseRepository.QuerySingleOrDefault<Customer>(connectionString,
                    "SELECT Id, Name, Address, Phone, Country FROM Customer WHERE Name = @Name;", new { Name = name });
            }
            catch (DataRepositoryException ex)
            {
                _log.LogError($"Exception at GetCustomer: {ex.Message} - StackTrace is: { ex }", ex.Message, ex);
                return null;
            }
        }
    }
}
