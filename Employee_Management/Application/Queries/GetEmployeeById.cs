using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetEmployeeByIdQuery : IRequest<Employee>
    {
        public string EmployeeId { get; set; }

        public GetEmployeeByIdQuery(string employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
