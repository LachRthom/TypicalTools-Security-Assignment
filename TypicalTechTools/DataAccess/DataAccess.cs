using Microsoft.EntityFrameworkCore;
using TypicalTechTools.Models;


namespace TypicalTechTools.DataAccess
{
    // Defining a class ApplicationDbContext that inherits from DbContext
    public class ApplicationDbContext : DbContext
    {
        // Declaring a private field to hold the hosting environment, this allows the class to interact
        // with the file system, in our case files in the applications root directory
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor for the ApplicationDbContext class.
        // Takes DbContextOptions and IWebHostEnvironment as parameters.
        // DbContextOptions represents the options for the database context, 
        // while IWebHostEnvironment represents the hosting environment of the web application.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IWebHostEnvironment webHostEnvironment)
            : base(options)
        {
            // Assigning the provided hosting environment to the private field.
            _webHostEnvironment = webHostEnvironment;
        }

        // This is our link to the the Students and Comments tables in the SQL database
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        // This methods runs upon the creation of the tables, and is used to seed the tables with initial data
        // In this case we are using it to migrate the data from a .CSV file into an SQL database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Letting EF Core know that the ProductPrice field in the Products table should be a decimal type 
            // with a precision of 18 digits and a scale of 2, this should be correct to handle any priced product
            modelBuilder.Entity<Product>().Property(p => p.ProductPrice).HasColumnType("decimal(18, 2)");

            // Configure ProductId as the primary key for the Products table
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);

            // Construct the file path relative to the wwwroot directory
            var productFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "data\\Products.csv");

            // Read data from CSV file
            var products = ReadProductsFromCsv(productFilePath);

            // Seed the Products table with data from the CSV file
            modelBuilder.Entity<Product>().HasData(products);

            // Configure ProductId as the primary key for the Products table
            modelBuilder.Entity<Comment>().HasKey(p => p.CommentId);

            // Construct the file path relative to the wwwroot directory
            var commentFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "data\\Comments.csv");

            // Read data from CSV file
            var comments = ReadCommentsFromCsv(commentFilePath);

            // Seed the Products table with data from the CSV file
            modelBuilder.Entity<Comment>().HasData(comments);

            // Configure AdminId as the primary key for the Products table
            modelBuilder.Entity<AdminUser>().HasKey(p => p.AdminId);

            // Seed the Admin User table with a default Admin entry
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    AdminId = 1,
                    Username = "Admin",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("SecurePassword123!"),
                    Role = "ADMIN"
                });
        }


        // Method to read products data from a CSV file
        private List<Product> ReadProductsFromCsv(string filePath)
        {
            var products = new List<Product>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var product = new Product
                    {
                        ProductId = int.Parse(values[0]),
                        ProductName = values[1],
                        ProductPrice = decimal.Parse(values[2]),
                        ProductDescription = values[3]
                    };

                    products.Add(product);
                }
            }
            return products;
        }
        // Method to read comments data from a CSV file
        private List<Comment> ReadCommentsFromCsv(string filePath)
        {
            var comments = new List<Comment>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var comment = new Comment
                    {
                        CommentId = int.Parse(values[0]),
                        CommentText = values[1],
                        ProductId = int.Parse(values[2])
                    };

                    comments.Add(comment);
                }
            }
            return comments;
        }
    }
}
