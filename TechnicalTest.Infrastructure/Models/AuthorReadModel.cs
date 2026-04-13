namespace TechnicalTest.Infrastructure.Models
{
    public class AuthorReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
    }
}