using Application.DTOs.LeaveAllocation;
using Application.Features.LeaveAllocations.Request.Commands;
using Application.Features.LeaveAllocations.Request.Queries;
using Application.Features.LeaveRequest.Request.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveAllocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<LeaveAllocationsController>
        [HttpGet]
        public async Task<ActionResult<List<LeaveAllocationDto>>> Get()
        {
            var leaveAllocation = await _mediator.Send(new GetLeaveAllocationListRequest());

            return Ok(leaveAllocation);
        }

        // GET api/<LeaveAllocationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveAllocationDto>> Get(int id)
        {
            var leaveAllocationDetail = await _mediator.Send(new GetLeaveAllocationDetailRequest { Id = id });

            return Ok(leaveAllocationDetail);
        }

        // POST api/<LeaveAllocationsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveAllocationDto leaveAllocation)
        {
            var command = new CreateLeaveAllocationCommand { LeaveAllocationDto = leaveAllocation };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        // PUT api/<LeaveAllocationsController>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateLeaveAllocationDto updateLeaveAllocationCommand)
        {
            var command = new UpdateLeaveAllocationCommand { LeaveAllocationDto = updateLeaveAllocationCommand };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        // DELETE api/<LeaveAllocationsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteLeaveAllocationCommand { Id = id });

            return Ok(response);
        }
    }
}