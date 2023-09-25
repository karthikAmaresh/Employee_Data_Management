using Application.Interfaces;
using Application.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeById, bool>
    {
        private readonly IEmployeeService _employeeService;

        public DeleteEmployeeHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Task<bool> Handle(DeleteEmployeeById request, CancellationToken cancellationToken)
        {
            return _employeeService.DeleteEmployee(request.EmployeeId);
        }
    }
}
