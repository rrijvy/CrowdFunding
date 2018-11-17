using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CrowdFunding.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrowdFunding.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment environment,
            ApplicationDbContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _emailSender = emailSender;
            _context = context;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required, Display(Name = "First name")]
            public string FName { get; set; }

            [Required, Display(Name = "Last name")]
            public string LName { get; set; }

            [Required, Display(Name = "Permanent Address")]
            public string PermanentAddress { get; set; }

            [Required, Display(Name = "Present Address")]
            public string PresentAddress { get; set; }

            public string Image { get; set; }

            [Display(Name = "Date of birth"), DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Required]
            public string NID { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FName = user.FName,
                LName = user.LName,
                NID = user.NID,
                PermanentAddress = user.ParmanantAddress,
                PresentAddress = user.PresentAddress,
                Image = user.Image
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile userAvater)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (!(userAvater == null))
            {
                var fileName = ContentDispositionHeaderValue.Parse(userAvater.ContentDisposition).FileName.Trim('"').Replace(" ", string.Empty);
                var fileExtension = Path.GetExtension(userAvater.FileName).ToLower();
                if ((fileExtension == ".jpg") || (fileExtension == ".jpeg") || (fileExtension == ".png") || (fileExtension == ".bmp"))
                {
                    using (var stream = new FileStream(GetPath(fileName, user.Email), FileMode.Create))
                    {
                        await userAvater.CopyToAsync(stream);
                    }
                }
                    

                user.Image = fileName;
            }

            user.FName = Input.FName;
            user.LName = Input.LName;
            user.NID = Input.NID;
            user.PresentAddress = Input.PresentAddress;
            user.ParmanantAddress = Input.PermanentAddress;            

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                user.Email = Input.Email;
                //var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                //if (!setEmailResult.Succeeded)
                //{
                //    var userId = await _userManager.GetUserIdAsync(user);
                //    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                //}
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
                //var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                //if (!setPhoneResult.Succeeded)
                //{
                //    var userId = await _userManager.GetUserIdAsync(user);
                //    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                //}
            }



            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        private string GetPath(string fileName, string userEmail)
        {
            string path = _environment.WebRootPath + "\\images\\" + userEmail + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }
    }
}
