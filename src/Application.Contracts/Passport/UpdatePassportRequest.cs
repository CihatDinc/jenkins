namespace Nebim.Era.Plt.Comm.Customer.Application.Contracts;

using System;

public class UpdatePassportRequest : PassportRequest, IRequest<PassportResponse>
{
    public Guid Id { get; set; }
}
