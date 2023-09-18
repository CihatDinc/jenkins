using Nebim.Era.Plt.Data;

namespace Nebim.Era.Plt.Comm.Customer.Domain;

using Application.Contracts;

public interface ICustomerRepository : ICrudRepository<Customer, Guid>
{
    Task<Customer?> GetAsTracking(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetAsNoTracking(Guid id, CancellationToken cancellationToken = default);
    Task<List<Customer>> GetTemporaryCustomers(ImportCustomerRequest request, CancellationToken cancellationToken = default);
    Task BulkCreate(IEnumerable<Customer> salesPersons, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPassportNumber(string passportNumber, CancellationToken cancellationToken = default);
    Task<List<Customer>> GetBy(GetCustomerByPassportInfoRequest request, CancellationToken cancellationToken);
}
