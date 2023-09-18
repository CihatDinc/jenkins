using Nebim.Era.Plt.Data;

namespace Nebim.Era.Plt.Comm.Customer.Domain;

public interface IPassportRepository : ICrudRepository<Passport, Guid>
{
    public Task<Passport> GetByCustomerId(Guid customerId, CancellationToken cancellationToken);
    
}
