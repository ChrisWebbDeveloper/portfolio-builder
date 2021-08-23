using FluentValidation;

namespace PortfolioBuilder.ModelValidators
{
    public static class PhoneNoValidator
    {
        public static IRuleBuilderOptions<T, string> PhoneNo<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$").WithMessage("'{PropertyName}' is not a valid phone number");
        }
    }
}
