using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
    {
        private readonly IEmployeeService _employeeService;

        public UpdateEmployeeHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public Task<Employee> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            return _employeeService.UpdateEmployee(command);
        }
    }
}
