using TechnicalTest.Application.DTOs;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class PostDtoBuilder
    {
        private readonly Guid _id;
        private readonly Guid _authorId;
        private readonly string _title;
        private readonly string _description;
        private readonly string _content;
        private readonly AuthorDto? _author;

        public static PostDtoBuilder Default() => new();

        private PostDtoBuilder()
        {
            _id = Guid.NewGuid();
            _title = "Test title";
            _description = "Test description";
            _content = "Test content";

            _author = AuthorDtoBuilder.Default()
                .Build();
            _authorId = _author.Id;
        }

        public PostDto Build() => new(
            _id,
            _authorId,
            _title,
            _description,
            _content,
            _author);
    }
}
