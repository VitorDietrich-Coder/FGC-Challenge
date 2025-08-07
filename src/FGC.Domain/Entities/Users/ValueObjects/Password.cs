using Abp.Domain.Values;
using FGC.Domain.Core.Exceptions;


namespace FGC.Domain.Entities.Users.ValueObjects
{
    public class Password : ValueObject
    {
        private static readonly char[] SpecialCharacters = "!@#$%^&*(){}[];:,.<>?".ToCharArray();

        public string Hash { get; }

        protected Password() { }

        public Password(string password)
        {

            ValidatePasswordStrength(password);

            Hash = HashPassword(password);
        }


        public bool Challenge(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return VerifyPassword(password, Hash);
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 8 || password.Length > 100)
                throw new BusinessRulesException("Password must be between 8 and 100 characters.");

            if (!password.Any(char.IsLetter))
                throw new BusinessRulesException("Password must contain at least one letter.");

            if (!password.Any(char.IsDigit))
                throw new BusinessRulesException("Password must contain at least one number.");

            if (!password.Any(c => SpecialCharacters.Contains(c)))
                throw new BusinessRulesException($"Password must contain at least one special character: {new string(SpecialCharacters)}");
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Hash;
        }
    }
}
