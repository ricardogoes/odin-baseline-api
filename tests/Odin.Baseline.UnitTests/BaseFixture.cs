using Bogus;

namespace Odin.Baseline.UnitTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public static string GetInvalidUsersername()
            => "";

        public static bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;
    }
}
