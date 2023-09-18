namespace Nebim.Era.Plt.Comm.Customer.Service.HttpClient;

using System.Linq.Expressions;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Customer.HttpClient;

public static class CustomerApiExtensions
{
    public static Task<List<CustomerResponse>> Where(this ICustomersApi api, Expression<Func<CustomerResponse, bool>?> response)
    {
        return api.List();
    }
}
