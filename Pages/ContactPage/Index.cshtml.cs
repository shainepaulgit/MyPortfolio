using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Services;

namespace MyPortfolio.Pages.ContactPage
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly EmailSender _emailSender;
        public IndexModel(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [BindProperty]
        public ContactInput Input { get; set; }
        public async Task<IActionResult> OnPostAsync(ContactInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var messageBodyHtml = $@"
                                    <!DOCTYPE html>
                                    <html lang='en'>
                                    <head>
                                      <meta charset='UTF-8'>
                                      <title>Email</title>
                                    </head>
                                    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;'>
                                      <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 12px; border: 1px solid #ddd;'>
                                        <div style='margin-bottom: 20px;'>
                                          <h4 style='font-size: 20px; font-weight: 700; margin: 0;'>Zup Bossing,</h4>
                                          <h4 style='margin: 5px 0;'>{input.Name}</h4>
                                          <h5 style='color: #555; font-weight: 400; margin: 0;'>{input.Email}</h5>
                                        </div>
                                        <div>
                                          <p style='font-size: 15px; color: #333; line-height: 1.5;'>{input.Message}</p>
                                          <a href='mailto:{input.Email}' style='display: inline-block; margin-top: 20px; padding: 10px 25px; background-color: #007BFF; color: white; text-decoration: none; border-radius: 30px; font-size: 14px;'>📧 Reply Email</a>
                                        </div>
                                      </div>
                                    </body>
                                    </html>";

            var messageStatus = await _emailSender.SendEmailAsync(input.Email,false,input.Subject, messageBodyHtml);
            TempData["alert-message"] = messageStatus;
            return RedirectToPage();
        }
    }
}
