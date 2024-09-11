using Microsoft.AspNetCore.Mvc;
using TypicalTechTools.Models;
using TypicalTechTools.Models.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Ganss.Xss;

namespace TypicalTools.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly HtmlSanitizer _sanitizer;

        public AdminController(IAuthenticationRepository authenticationRepository, HtmlSanitizer sanitizer)
        {
            _authenticationRepository = authenticationRepository;
            _sanitizer = sanitizer;
        }

        public IActionResult AdminLogin([FromQuery] string ReturnUrl)
        {
            LoginDTO login = new LoginDTO
            {
                ReturnURL = string.IsNullOrWhiteSpace(ReturnUrl) ? "/Product/Index" : ReturnUrl
            };

            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginDTO loginDTO)
        {
            // Sanitize user input
            loginDTO.Username = _sanitizer.Sanitize(loginDTO.Username);
            loginDTO.Password = _sanitizer.Sanitize(loginDTO.Password);

            var user = _authenticationRepository.Authenticate(loginDTO);

            if (user == null)
            {
                ViewBag.LoginMessage = "Username or Password incorrect";
                return View();
            }

            // List of claims for the current user.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Create a ClaimsPrincipal with the user's role and username
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme));

            // Authentication properties, similar to middleware cookie settings
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                RedirectUri = loginDTO.ReturnURL
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                            principal, authProperties);

            return Redirect(loginDTO.ReturnURL);
        }

        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult AdminLogOff()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        public IActionResult AdminLoginSuccess()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult Create(CreateUserDTO createDTO)
        {
            // Sanitize user input
            createDTO.Username = _sanitizer.Sanitize(createDTO.Username);
            createDTO.Password = _sanitizer.Sanitize(createDTO.Password);
            createDTO.PasswordConfirmation = _sanitizer.Sanitize(createDTO.PasswordConfirmation);

            // Check for password match between the two fields
            if (!createDTO.Password.Equals(createDTO.PasswordConfirmation))
            {
                ViewBag.CreateUserError = "Passwords do not match";
                // Clear the password fields
                createDTO.Password = string.Empty;
                createDTO.PasswordConfirmation = string.Empty;
                return View(createDTO);
            }

            // Attempt to add the new user
            var newUser = _authenticationRepository.CreateAdminUser(createDTO);

            if (newUser == null)
            {
                ViewBag.CreateUserError = "Failed to create user account, user already exists";
                return View(createDTO);
            }

            // Clear the model state and confirm success
            ModelState.Clear();
            ViewBag.CreateUserConfirmation = "User account created successfully";
            return View();
        }
    }
}
