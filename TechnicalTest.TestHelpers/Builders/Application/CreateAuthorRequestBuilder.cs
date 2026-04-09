using TechnicalTest.Api.Models;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class CreateAuthorRequestBuilder
    {
        private readonly string _name;
        private readonly string _surname;

        public static CreateAuthorRequestBuilder Default() => new();

        private CreateAuthorRequestBuilder()
        {
            _name = "Test name";
            _surname = "Test surname";
        }

        public CreateAuthorRequest Build() => new(
            _name,
            _surname
        );
    }
}
