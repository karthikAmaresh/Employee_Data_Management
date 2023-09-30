using Application.Commands;
using Application.Handlers;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService( 
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName,
            ILogger<EmployeeService> logger)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
            _logger = logger;
        }
        public async Task<List<Employee>> GetEmployees(string queryString)
        {
            try
            {
                _logger.LogInformation($"GetEmployees :Employee Service Calling Azure Func GetEmployees");
                var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition(queryString));
                var results = new List<Employee>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employees");
                throw;
            }
            
        }

        public async Task<Employee> GetEmployee(string id)
        {
            try
            {
                _logger.LogInformation($"GetEmployee :Employee Service Calling Azure Func GetEmployee with id  : {id}");
                var response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) //For handling item not found and other exceptions
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employee with id {id}");
                throw ;
            }
        }

        public async Task<string> AddEmployee(AddEmployeeCommand employee)
        {
            try
            {
                _logger.LogInformation($"AddEmployee :Employee Service Calling Azure Func AddEmployee for employee {employee.firstName} {employee.lastName}");
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
            catch (Exception ex)
            {
                _logger.LogInformation($"ERROR {ex.Message} Calling AddEmployee for employee {employee.firstName} {employee.lastName}");

                throw;
            }
            
        }

        public async Task<Employee> UpdateEmployee(UpdateEmployeeCommand employee)
        {
            try
            {
                _logger.LogInformation($"UpdateEmployee :Employee Service Calling Azure Func UpdateEmployee for id {employee.id}");

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
            catch (Exception ex)
            {
                _logger.LogInformation($"ERROR {ex.Message} Calling UpdateEmployee for id {employee.id}");
                throw;
            }
            
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            try
            {
                _logger.LogInformation($"DeleteEmployee :Employee Service Calling Azure Func DeleteEmployee for id {id}");
                await _container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ERROR {ex.Message} Calling DeleteEmployee for id {id}");
                throw;
            }
            
        }

    }
}
