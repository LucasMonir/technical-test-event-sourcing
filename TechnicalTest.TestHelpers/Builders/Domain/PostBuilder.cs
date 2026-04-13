using TechnicalTest.Domain.Models;

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

        public Post Build()
        {
            return Post.Create(
                _id,
                authorId,
                _title,
                _description,
                _content
            );
        }
    }
}
