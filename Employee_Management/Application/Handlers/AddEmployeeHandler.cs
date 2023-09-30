using Application.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.Handlers
{
    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, string>
    {
        private readonly IEmployeeService _employeeService;

        public AddEmployeeHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Task<string> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
        {
            return _employeeService.AddEmployee(command);
        }
    }
}
