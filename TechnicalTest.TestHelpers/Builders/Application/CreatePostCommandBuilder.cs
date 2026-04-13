using TechnicalTest.Application.Commands;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class CreatePostCommandBuilder
    {
        private Guid? authorId;
        private readonly string _title;
        private readonly string _description;
        private readonly string _content;
        private AuthorModel? author;

        public static CreatePostCommandBuilder Default() => new();

        private CreatePostCommandBuilder()
        {
            authorId = null;
            _title = "Test title";
            _description = "Test description";
            _content = "Test content";
            author = null;
        }

        public CreatePostCommandBuilder WithAuthorId(Guid authorId)
        {
            this.authorId = authorId;
            return this;
        }

        public CreatePostCommandBuilder WithAuthor(AuthorModel author)
        {
            this.author = author;
            return this;
        }

        public CreatePostCommand Build() => new(
            authorId,
            _title,
            _description,
            _content,
            author
        );
    }
}
