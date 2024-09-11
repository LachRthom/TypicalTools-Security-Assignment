using Microsoft.AspNetCore.Authorization;
using TypicalTechTools.DataAccess;

namespace TypicalTechTools.Models.Repositories
{

    public class AuthenticationRepository : IAuthenticationRepository
    {

        private readonly ApplicationDbContext _context;

        public AuthenticationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public AdminUser Authenticate(LoginDTO loginDTO)
        {
            // Look for a user in the DB table that matches provided details
            var userDetails = _context.AdminUsers.Where(u => u.Username.Equals(loginDTO.Username))
                                                                        .FirstOrDefault();

            // handle no user match
            if (userDetails == null)
            {
                return null;
            }

            // check for password match
            if (BCrypt.Net.BCrypt.EnhancedVerify(loginDTO.Password, userDetails.Password)) 
            { 
                return userDetails;
            }

            // hande wrong password
            return null;
        }

        public AdminUser CreateAdminUser(CreateUserDTO createUserDTO)
        {
            // Check if username exists to avoid duplicate entries
            var userDetails = _context.AdminUsers.Where(u => u.Username.Equals(createUserDTO.Username))
                                                                        .FirstOrDefault();

            // If matching user exists, exit and return null
            if (userDetails != null)
            {
                return null;
            }

            // Create a new AdminUser using details from the DTO
            AdminUser user = new AdminUser
            {
                Username = createUserDTO.Username,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(createUserDTO.Password),
                Role = "GUEST"
            };

            // Add the newly created user to the DB
            _context.AdminUsers.Add(user);
            _context.SaveChanges();
            return user;
        }


    }
}
