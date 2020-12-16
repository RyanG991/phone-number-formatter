using PhoneNumberFormatter.Hashing.Passwords;

namespace PhoneNumberFormatter.Tests.Hashing.Passwords.PasswordHasherTests
{
    public abstract class PasswordHasherTestBase
    {
        protected PasswordHasher Sut;

        protected const string Password1 = "T3st";
        protected const string Reference1 = "1234";
        protected const string HashedPassword1 = "$2a$13$......................xsfdfuKS7koTF7sOTDvYtZVokL6HDsK";

        protected PasswordHasherTestBase()
        {
            Sut = new PasswordHasher();
        }
    }
}
