using Application.Interfaces;
using Application.Queries;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
    {
        private readonly IEmployeeService _employeeService;

        public GetEmployeeByIdHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            return _employeeService.GetEmployee(request.EmployeeId);
        }
    }
}
