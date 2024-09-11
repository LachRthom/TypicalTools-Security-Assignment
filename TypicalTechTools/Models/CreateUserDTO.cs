namespace TypicalTechTools.Models
{
    public class CreateUserDTO
    {

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;

    }
}
