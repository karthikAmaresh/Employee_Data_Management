using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class EmployeeEmailVaidator : AbstractValidator<AddEmployeeCommand>
    {
        public EmployeeEmailVaidator()
        {
            RuleFor(x => x.email).Must(ValidateEmail.IsValidEmail).WithMessage("Email is invalid");
        }

    }

    public class UpdateEmployeeEmailVaidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeEmailVaidator()
        {
            RuleFor(x => x.email).Must(ValidateEmail.IsValidEmail).WithMessage("Email is invalid");
        }
    }
}
