namespace TypicalTechTools.Models.Repositories
{
    public interface IAuthenticationRepository
    {
        AdminUser Authenticate(LoginDTO loginDTO);

        AdminUser CreateAdminUser(CreateUserDTO createUserDTO);

    }
}
