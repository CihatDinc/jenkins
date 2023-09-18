namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

using System;

public class DeletePassportRequest : IRequest
{
    public Guid Id { get; set; }
}
