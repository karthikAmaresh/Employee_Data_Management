using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UpdateEmployeeCommand : IRequest<Employee>
    {
        public string id { get; set; }
        public int age { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string eyeColor { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public string address { get; set; }
        public string about { get; set; }
    }
}
