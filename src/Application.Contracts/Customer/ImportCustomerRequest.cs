namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using System;
using System.Collections.Generic;

public class ImportCustomerRequest : IRequest<List<CustomerResponse>>
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public string TableName { get; set; }
}
