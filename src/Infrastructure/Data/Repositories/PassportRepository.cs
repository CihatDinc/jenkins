namespace Infrastructure.Data.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Data.EntityFramework;

public class PassportRepository : CrudRepository<CustomerDbContext, Passport, Guid>, IPassportRepository
{
    private readonly CustomerDbContext _dbContext;

    public PassportRepository(CustomerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Passport> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Passports.FirstOrDefaultAsync(t => t.CustomerId == customerId);
        return result;
    }
}
