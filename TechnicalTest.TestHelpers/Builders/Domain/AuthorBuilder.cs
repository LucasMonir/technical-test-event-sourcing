using TechnicalTest.Domain;

namespace TechnicalTest.TestHelpers.Builders.Domain
{
    public class AuthorBuilder
    {
        readonly Guid _id;
        readonly string _name;
        readonly string _surname;

        public static AuthorBuilder Default() => new();

        private AuthorBuilder()
        {
            _id = Guid.NewGuid();
            _name = "Test name";
            _surname = "Test surname";
        }

        public Author Build() => new()
        {
            Id = _id,
            Name = _name,
            Surname = _surname
        };
    }
}
