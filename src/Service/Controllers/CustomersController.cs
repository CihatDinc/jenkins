namespace Service.Controllers;

using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Core;

[ApiController]
[Route("[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns customers
    /// </summary>
    /// <remarks>
    /// For detailed info about filter, syntax check the below link
    /// https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#97-filtering
    /// </remarks>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] ListCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns customer by Id
    /// </summary>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustomerRequest { Id = id }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns customer by passport number
    /// </summary>
    [HttpGet(":passportInfo")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPassportInfo([FromQuery] GetCustomerByPassportInfoRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create customer
    /// </summary>
    /// <response code="201">Returns the newly created item Uri</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Created(new Uri($"{Request.Path}/{result.Id}", UriKind.Relative), result);
    }

    /// <summary>
    /// Update customer
    /// </summary>
    [HttpPut]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Delete customer by Id
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCustomerRequest { Id = id }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Add address to a customer; you can add a default address if only there is not any address before.
    /// </summary>
    /// <response code="201">Returns the newly created item Uri</response>
    [HttpPost("{customerId:guid}/address")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddCustomerAddress([FromBody] AddCustomerAddressRequest request, [FromRoute] Guid customerId, CancellationToken cancellationToken)
    {
        request.CustomerId = customerId;
        var result = await _mediator.Send(request, cancellationToken);
        return Created(new Uri($"{Request.Path}/{result.Id}", UriKind.Relative), result);
    }

    /// <summary>
    /// Delete address from a customer; if you delete the default address, the first address in the list will be set as default.
    /// </summary>
    [HttpDelete("{customerId:guid}/address/{customerAddressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomerAddress([FromRoute] Guid customerId, [FromRoute] Guid customerAddressId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCustomerAddressRequest() { CustomerId = customerId, CustomerAddressId = customerAddressId }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Update address of a customer; you can set 'IsDefault' as true if only it was false before. Otherwise, update operation fails.
    /// </summary>
    [HttpPut("{customerId:guid}/address/{customerAddressId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateCustomerAddress([FromBody] UpdateCustomerAddressRequest request, Guid customerId, Guid customerAddressId,
        CancellationToken cancellationToken)
    {
        request.CustomerId = customerId;
        request.CustomerAddressId = customerAddressId;
        await _mediator.Send(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("/customers/import")]
    [ProducesResponseType(typeof(List<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportCustomer([FromBody] ImportCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
