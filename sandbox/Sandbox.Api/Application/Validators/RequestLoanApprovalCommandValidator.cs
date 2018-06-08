using FluentValidation;
using Sandbox.Api.Application.Commands;

namespace Sandbox.Api.Application.Validators
{
    public class RequestLoanApprovalCommandValidator : AbstractValidator<RequestFluentLoanApprovalCommand>
    {
        public RequestLoanApprovalCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Value).NotNull().NotEmpty().Length(5, 10);
        }
    }
}
