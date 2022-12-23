using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IBook.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IdentityUser> _logger;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ILogger<IdentityUser> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        [HttpPost]
        public void SendEmail( string _description,string toAddress)
        {

            //MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("17110399@student.hcmute.edu.vn");
            //mail.To.Add("vantrieutqd@gmail.com");
            //mail.Subject = "Recovery Password for account ";
            //mail.Body = _description;

            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.gmail.com";
            //NetworkCredential ntcd = new NetworkCredential();
            //ntcd.UserName = "17110399@student.hcmute.edu.vn";
            //ntcd.Password = "Qt0169477275";
            //smtp.Credentials = ntcd;
            //smtp.EnableSsl = true;
            //smtp.Port = 587;
            //smtp.Send(mail);


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("17110399@student.hcmute.edu.vn");
            mail.To.Add(toAddress);
            mail.Subject = "hello";
            mail.Body = _description;
            //Attachment attachment = new Attachment("hello");
            //mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("17110399@student.hcmute.edu.vn", "Qt0169477275");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);








        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passWordResetLink = Url.Action("ResetPassword", "Account", new { email = Input.Email, token = token }, Request.Scheme);
                    _logger.Log(LogLevel.Warning, passWordResetLink);
                    // Don't reveal that the user does not exist or is not confirmed
                    SendEmail(passWordResetLink.ToString(),Input.Email);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
