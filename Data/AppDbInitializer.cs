namespace OnlineBookStore.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                // Seed Books
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new List<Models.Book>()
                    {
                        new Models.Book()
                        {
                            Title = "The Great Gatsby",
                            Author = "F. Scott Fitzgerald",
                            Description = "A novel set in the Jazz Age that tells the story of Jay Gatsby's unrequited love for Daisy Buchanan.",
                            Price = 10.99M,
                            ImageUrl = "https://example.com/images/greatgatsby.jpg"
                        },
                        new Models.Book()
                        {
                            Title = "To Kill a Mockingbird",
                            Author = "Harper Lee",
                            Description = "A novel about the serious issues"
                        },
                    });
                    context.SaveChanges();
                }

                // Seed Users
                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<Models.User>()
                    {
                        new Models.User()
                        {
                            FullName = "john_doe",
                            Email = "john_doe@gmail.com",
                            Password = "hashed_password_1"
                        },
                    });
                context.SaveChanges();
                }

            }
        }
    }
}
