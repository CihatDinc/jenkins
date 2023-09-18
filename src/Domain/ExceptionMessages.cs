namespace Nebim.Era.Plt.Comm.Customer.Domain;

public static class ExceptionMessages
{
    public static string CouldNotFindCustomerById(Guid customerId)
    {
        return $"Couldn't find Customer with 'Customer Id'={customerId}";
    }

    public static string CouldNotFindCustomerAddressById(Guid customerAddressId)
    {
        return $"Couldn't find CustomerAddress with 'CustomerAddressId'={customerAddressId}";
    }

    public const string CustomerCodeRequired = "'Customer Code' must not be empty!";
    public const string CustomerIdRequired = "'Customer Id' must not be empty!";

    public static string CouldNotFindPassportById(Guid id)
    {
        return $"Couldn't find Passport with 'Id'={id}";
    }

    public static string CouldNotFindPassportByCustomerId(Guid customerId)
    {
        return $"Couldn't find Passport with given 'CustomerId'={customerId}";
    }

    public static string CouldNotFindCustomerByPassportNumber(string passportNumber)
    {
        return $"Couldn't find Customer with given 'PassportNumber'={passportNumber}";
    }

    public static string CannotHaveMultiplePassportDefined(Guid customerId)
    {
        return $"Customer with given Id={customerId} already have a passport defined.";
    }
}
