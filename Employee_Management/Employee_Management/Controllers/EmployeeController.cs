using Application.Commands;
using Application.Queries;
using Application.Validators;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(IMediator mediator , ILogger<EmployeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            _logger.LogInformation($"Retrieving Employee List");
            var result = await _mediator.Send(new GetAllEmployees());
            return Ok(result);
        }
        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            _logger.LogInformation($"Retrieving Employee of id {id}");
            var employeeId = id;
            return Ok(await _mediator.Send(new GetEmployeeByIdQuery(employeeId)));
        }
        // POST api/items
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeCommand command)
        {
            _logger.LogInformation($"Creating Employee {command.firstName} {command.lastName}");
            var validate = new EmployeeEmailVaidator();
            var validationResult = validate.Validate(command);
            if (validationResult.IsValid)
            {
                try
                {
                    if (command.company != null
                    && command.email != null)
                    {
                        var address = new System.Net.Mail.MailAddress(command.email);
                        var emailDomain = address.Host;

                        var isDomainExists = emailDomain.Equals(command.company + ".com", StringComparison.OrdinalIgnoreCase);
                        if (!isDomainExists)
                        {
                            ModelState.AddModelError(nameof(Employee.email), "please provide email provided by company domain!");
                        }

                    }
                    if (!ModelState.IsValid)
                    {
                        return UnprocessableEntity(ModelState);
                    }
                    var result = await _mediator.Send(command);
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(x => x.ErrorMessage));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeCommand command)
        {
            _logger.LogInformation($"Updating Employee with id {command.id}");
            var validate = new UpdateEmployeeEmailVaidator();
            var validationResult = validate.Validate(command);
            if (validationResult.IsValid)
            {
                try
                {
                    if (command.company != null
                    && command.email != null)
                    {
                        var address = new System.Net.Mail.MailAddress(command.email);
                        var emailDomain = address.Host;

                        var isDomainExists = emailDomain.Equals(command.company + ".com", StringComparison.OrdinalIgnoreCase);
                        if (!isDomainExists)
                        {
                            ModelState.AddModelError(nameof(Employee.email), "please provide email provided by company domain!");
                        }

                    }
                    if (!ModelState.IsValid)
                    {
                        return UnprocessableEntity(ModelState);
                    }
                    var result = await _mediator.Send(command);
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(x => x.ErrorMessage));
            }
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            _logger.LogInformation($"Deleting Employee with id {id}");
            try
            {
                var result = await _mediator.Send(new DeleteEmployeeById(id));
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}

