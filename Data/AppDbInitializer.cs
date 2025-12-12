using OnlineBookStore.Models;

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
                            Name = "Holly Jackson",
                            Biography = "British author known for the A Good Girl’s Guide to Murder series."
                        },
                        new Models.Authors()
                        {
                            Name = "Agatha Christie",
                            Biography = "The Queen of Crime, famous for Hercule Poirot and Miss Marple mysteries."
                        },
                        new Models.Authors()
                        {
                            Name = "Charles Dickens",
                            Biography = "One of the greatest novelists of the Victorian era."
                        }
                    });

                    context.SaveChanges();
                }

                // 4.Seed Categories
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>
                    {
                        new Category { Name = "Crime" },
                        new Category { Name = "Mystery" },
                        new Category { Name = "Classic" },
                        new Category { Name = "Self-help" }
                    });
                    context.SaveChanges();
                }

                // 2. Seed Books
                if (!context.Books.Any())
                {
                    var hollyJackson = context.Authors.First(a => a.Name == "Holly Jackson");
                    var agathaChristie = context.Authors.First(a => a.Name == "Agatha Christie");
                    var charlesDickens = context.Authors.First(a => a.Name == "Charles Dickens");

                    context.Books.AddRange(new List<Models.Book>()
                    {
                        new Models.Book()
                        {
                            Title = "A Good Girl's Guide to Murder",
                            AuthorID = hollyJackson.AuthorID,
                            CategoryId = 2, //Mystery
                            Price = 14.99M,
                            Stock = 25,
                            Sales = 150,
                            Description = "A teen reopens a closed murder case, uncovering dangerous secrets.",
                            ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e2/A_Good_Girl%27s_Guide_to_Murder.jpg"
                        },
                        new Models.Book()
                        {
                            Title = "Death on the Nile",
                            AuthorID = agathaChristie.AuthorID,
                            CategoryId = 1, //Crime
                            Price = 13.50M,
                            Stock = 18,
                            Sales =50,
                            Description = "Hercule Poirot investigates a murder aboard a Nile cruise.",
                            ImageUrl = "https://m.media-amazon.com/images/I/916EaU54GlL._AC_UF894,1000_QL80_.jpg"
                        },
                        new Models.Book()
                        {
                            Title = "Oliver Twist",
                            AuthorID = charlesDickens.AuthorID,
                            CategoryId = 3, //Classic
                            Price = 11.00M,
                            Stock = 30,
                            Sales =200,
                            Description = "A young orphan faces the harsh realities of Victorian London.",
                            ImageUrl = "https://m.media-amazon.com/images/M/MV5BMTg4MjAxMTg5N15BMl5BanBnXkFtZTcwODIzNjEzMg@@._V1_FMjpg_UX1000_.jpg"
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
