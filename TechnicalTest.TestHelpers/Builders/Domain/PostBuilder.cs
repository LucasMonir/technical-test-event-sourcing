using TechnicalTest.Domain;

namespace TechnicalTest.TestHelpers.Builders.Domain
{
    public class PostBuilder
    {
        readonly Guid _id;
        Guid authorId;
        readonly string _title;
        readonly string _description;
        readonly string _content;

        public static PostBuilder Default() => new();

        private PostBuilder()
        {
            _id = Guid.NewGuid();
            authorId = Guid.NewGuid();
            _title = "Test title";
            _description = "Test description";
            _content = "Test content";
        }

        public PostBuilder WithAutorId(Guid id)
        {
            authorId = id;
            return this;
        }

        public Post Build() => new()
        {
            Id = _id,
            AuthorId = authorId,
            Title = _title,
            Description = _description,
            Content = _content
        };
    }
}
