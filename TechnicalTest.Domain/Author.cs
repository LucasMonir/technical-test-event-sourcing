namespace TechnicalTest.Domain
{
    public class Author
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }

        public static Author Create(string name, string surname)
        {
            return new Author()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Surname = surname
            };
        }
    }
}
