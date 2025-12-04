namespace OnlineBookStore.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();


                // 1. Seed Authors
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(new List<Models.Authors>()
                    {
                        new Models.Authors()
                        {
                            Name = "F. Scott Fitzgerald",
                            Biography = "An American novelist and short story writer, widely regarded as one of the greatest American writers of the 20th century."
                        },
                        new Models.Authors()
                        {
                            Name = "Harper Lee",
                            Biography = "An American novelist best known for her novel To Kill a Mockingbird."
                        }
                    });

                    context.SaveChanges();
                }

                // 2. Seed Books
                if (!context.Books.Any())
                {
                    var fitzgerald = context.Authors.First(a => a.Name == "F. Scott Fitzgerald");
                    var harperLee = context.Authors.First(a => a.Name == "Harper Lee");

                    context.Books.AddRange(new List<Models.Book>()
                    {
                        new Models.Book()
                        {
                            Title = "The Great Gatsby",
                            AuthorID = fitzgerald.AuthorID,
                            Category = "Classic",
                            Price = 10.99M,
                            Stock = 20,
                            Description = "A novel set in the Jazz Age that tells the story of Jay Gatsby's love for Daisy Buchanan.",
                            ImageUrl = "https://example.com/images/greatgatsby.jpg"
                        },
                        new Models.Book()
                        {
                            Title = "To Kill a Mockingbird",
                            AuthorID = harperLee.AuthorID,
                            Category = "Novel",
                            Price = 12.50M,
                            Stock = 15,
                            Description = "A powerful novel that deals with themes of racial injustice and childhood innocence.",
                            ImageUrl = "https://example.com/images/mockingbird.jpg"
                        }
                    });

                    context.SaveChanges();
                }

                
                // 3. Seed Users
                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<Models.User>()
                    {
                        new Models.User()
                        {
                            FullName = "John Doe",
                            Email = "john_doe@gmail.com",
                            Password = "hashed_password_1"
                        }
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
