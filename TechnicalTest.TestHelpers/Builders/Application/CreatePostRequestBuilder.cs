using TechnicalTest.Api.Models;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class CreatePostRequestBuilder
    {
        private readonly string _title;
        private readonly string _description;
        private readonly string _content;
        private Guid? authorId;
        private CreateAuthorRequest? authorRequest;

        public static CreatePostRequestBuilder Default() => new();

        private CreatePostRequestBuilder()
        {
            _title = "Test title";
            _description = "Test description";
            _content = "Test content";
            authorId = null;
            authorRequest = null;
        }

        public CreatePostRequestBuilder WithAuthorId(Guid authorId)
        {
            this.authorId = authorId;
            return this;
        }

        public CreatePostRequestBuilder WithAuthor(CreateAuthorRequest authorRequest)
        {
            this.authorRequest = authorRequest;
            return this;
        }

        public CreatePostRequest Build() => new(
            authorId,
            _title,
            _description,
            _content,
            authorRequest);
    }
}
