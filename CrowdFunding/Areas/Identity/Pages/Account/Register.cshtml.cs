using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CrowdFunding.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using CrowdFunding.Models;
using CrowdFunding.Services;

namespace CrowdFunding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICustomizedId _customId;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICustomizedId customId)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
            _customId = customId;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public int CountryId { get; set; }
            public string NID { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            [Display(Name = "Date of birth"), DataType(DataType.Date)]
            public DateTime DateofBirth { get; set; }
            public string PresentAddress { get; set; }
            public string ParmanantAddress { get; set; }        }

        public void OnGet(string returnUrl = null)
        {
            ViewData["Country"] = new SelectList(_context.Countries, "Id", "Name");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string name, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Identity/Account/Manage");
            if (ModelState.IsValid)
            {
                if (name == "Entreprenuer")
                {
                    var user = new Entrepreneur
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        CountryId = Input.CountryId,
                        NID = Input.NID,
                        FName = Input.FName,
                        LName = Input.LName,
                        DateofBirth = Input.DateofBirth,
                        PresentAddress = Input.PresentAddress,
                        ParmanantAddress = Input.ParmanantAddress,
                    };
                    user.EntrepreneurCustomizedId = _customId.EntreprenuerCustomId(user);

                    var result = await _userManager.CreateAsync(user, Input.Password);

                    await _userManager.AddToRoleAsync(user, "Entreprenuer");

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                           // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    var user = new Investor
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        CountryId = Input.CountryId,
                        NID = Input.NID,
                        FName = Input.FName,
                        LName = Input.LName,
                        DateofBirth = Input.DateofBirth,
                        PresentAddress = Input.PresentAddress,
                        ParmanantAddress = Input.ParmanantAddress
                    };
                    user.InvestorCustomizedId = _customId.InvestorCustomId(user);
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    await _userManager.AddToRoleAsync(user, "Investor");
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
