namespace Application;

using FluentValidation;
using Infrastructure.Mapper;
using Nebim.Era.Plt.Comm.Customer.Application.Contracts;
using Nebim.Era.Plt.Comm.Customer.Domain;
using Nebim.Era.Plt.Core;
using Nebim.Era.Plt.Core.Serialization.Json;
using Nebim.Era.Plt.Core.Validators.FluentValidationExtensions;

public class ImportCustomerHandler : IRequestHandler<ImportCustomerRequest, List<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerMapper _mapper;

    public ImportCustomerHandler(ICustomerRepository customerRepository, ICustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<List<CustomerResponse>> Handle(ImportCustomerRequest request, CancellationToken cancellationToken)
    {
        var errors = new List<Customer>();
        var result = await _customerRepository.GetTemporaryCustomers(request, cancellationToken);
        var mapped = _mapper.ToCustomers(result);
        var response = _mapper.ToCustomerResponse(mapped);
        var validator = new ImportCustomerValidator();
        foreach (var customer in mapped)
        {
            var validation = await validator.ValidateAsync(customer, cancellationToken);
            if (!validation.IsValid)
            {
                errors.Add(customer);
            }
        }
        if (errors.Any())
        {
            var obj = JsonSerde.Serialize(errors);
            throw new EraValidationException(obj);
        }
        await _customerRepository.BulkCreate(mapped, cancellationToken);
        return response;
    }
}

public class ImportCustomerValidator : AbstractValidator<Customer>
{
    public ImportCustomerValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
        RuleFor(t => t.Email).CustomEmailValidator().When(t => !string.IsNullOrWhiteSpace(t.Email));
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
        RuleFor(x => x.Addresses).Must(collection => collection.Any()).WithMessage("At least 1 Customer Address is required.");
        RuleFor(x => x.Addresses).Must(collection => collection.Count(o => o.IsDefault) < 2).WithMessage("You cannot add default address more than 1.");
        RuleFor(t => t.BirthDate).CustomBirthDateValidator().When(t => t.BirthDate.HasValue);
    }
}
