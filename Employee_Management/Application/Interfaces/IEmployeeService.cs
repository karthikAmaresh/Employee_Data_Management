using Application.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployees(string query);
        Task<Employee> GetEmployee(string id);
        Task<string> AddEmployee(AddEmployeeCommand employee);
        Task<Employee> UpdateEmployee(UpdateEmployeeCommand employee);
        Task<bool> DeleteEmployee(string id);

    }
}
