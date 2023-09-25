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
    public class GetEmployeeListHandler : IRequestHandler<GetAllEmployees, List<Employee>>
    {
        private readonly IEmployeeService _employeeService;

        public GetEmployeeListHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Task<List<Employee>> Handle(GetAllEmployees request, CancellationToken cancellationToken)
        {
            var queryString = "SELECT * FROM c";
            return _employeeService.GetEmployees(queryString);
        }
    }
}
