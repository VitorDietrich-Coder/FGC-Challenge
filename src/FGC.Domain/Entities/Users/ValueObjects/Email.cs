using Abp.Domain.Values;
using FGC.Domain.Core.Exceptions;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FGC.Domain.Entities.Users.ValueObjects
{
    public class Email : ValueObject
    {
        public string Address { get; }

        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new BusinessRulesException("Email address cannot be empty.");

            address = address.Trim().ToLowerInvariant();

            if (address.Length > 100)
                throw new BusinessRulesException("Email must be less than or equal to 100 characters.");

            if (!IsValidEmail(address))
                throw new BusinessRulesException("Invalid email address format.");

            Address = address;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return EmailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
        }
        public override string ToString() => Address;
    }
}
