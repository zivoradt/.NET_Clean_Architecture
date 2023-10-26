using Application.DTOs.LeaveType;
using Application.Features.LeaveTypes.Request.Commands;
using Application.Features.LeaveTypes.Request.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<LeaveTypesController>
        [HttpGet]
        public async Task<ActionResult<List<LeaveTypeDto>>> Get()
        {
            var leaveTypes = await _mediator.Send(new GetLeaveTypeListRequest());

            return leaveTypes;
        }

        // GET api/<LeaveTypesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveTypeDto>> Get(int id)
        {
            var leaveType = await _mediator.Send(new GetLeaveTypeDetailRequest { Id = id });

            return (leaveType);
        }

        // POST api/<LeaveTypesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveTypeDto leaveTypeDto)
        {
            var command = new CreateLeaveTypeCommand { LeaveTypeDto = leaveTypeDto };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        // PUT api/<LeaveTypesController>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] LeaveTypeDto leaveType)
        {
            var command = new UpdateLeaveTypeCommand { LeaveTypeDto = leaveType };

            await _mediator.Send(command);

            return NoContent();
        }

        // DELETE api/<LeaveTypesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveTypeCommand { Id = id };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}