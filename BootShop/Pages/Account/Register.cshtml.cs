using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BootShop.Data;
using BootShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System;

namespace BootShop.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public RegisterModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [MaxLength(255)]
            public string FullName { get; set; }

            [MaxLength(20)]
            public string? PhoneNumber { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var hashedPassword = HashPassword(Input.Password);

            var user = new User
            {
                Email = Input.Email,
                Password = hashedPassword,
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            await SendThankYouEmail(user);

            return RedirectToPage("/Account/Login");
        }

        private async Task SendThankYouEmail(User user)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("----------------");
                mail.To.Add(user.Email);
                mail.Subject = "Thank you for registering!";
                mail.Body = $"Dear {user.FullName},\n\nThank you for registering on our website. We appreciate your interest and hope you enjoy our services.\n\nBest regards,\nThe Team";
                using (SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new System.Net.NetworkCredential("--------", "--------");
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}