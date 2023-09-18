namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

using System;

public class GetPassportRequest : IRequest<PassportResponse>
{
    public Guid Id { get; set; }
}
