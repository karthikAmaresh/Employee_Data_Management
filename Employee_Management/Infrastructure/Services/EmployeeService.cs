using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private Container _container;

        public EmployeeService( 
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }
        public async Task<List<Employee>> GetEmployees(string queryString)
        {
            var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition(queryString));
            var results = new List<Employee>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Employee> GetEmployee(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<string> AddEmployee(AddEmployeeCommand employee)
        {
            Guid newId = Guid.NewGuid();
            Employee employeeDetails = new Employee()
            {
                id = newId.ToString(),
                about = employee.about,
                address = employee.address,
                age = employee.age,
                company = employee.company,
                email = employee.email,
                eyeColor = employee.eyeColor,
                firstName = employee.firstName,
                lastName = employee.lastName,
                phone = employee.phone
            };
            await _container.CreateItemAsync(employeeDetails, new PartitionKey(employeeDetails.id));
            return newId.ToString();
        }

        public async Task<Employee> UpdateEmployee(UpdateEmployeeCommand employee)
        {
            Employee employeeDetails = new Employee()
            {
                id = employee.id,
                about = employee.about,
                address = employee.address,
                age = employee.age,
                company = employee.company,
                email = employee.email,
                eyeColor = employee.eyeColor,
                firstName = employee.firstName,
                lastName = employee.lastName,
                phone = employee.phone
            };
            await _container.UpsertItemAsync(employeeDetails, new PartitionKey(employeeDetails.id));
            return employeeDetails;
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            await _container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
            return true;
        }

    }
}
