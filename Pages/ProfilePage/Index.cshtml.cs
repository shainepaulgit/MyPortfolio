using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Repositories.Contracts;
using MyPortfolio.Models.Services;
using System.Runtime.CompilerServices;

namespace MyPortfolio.Pages.ProfilePage
{
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly FileHandling _fileHandling;
        public IndexModel(
            SignInManager<AppIdentityUser> signInManager,
            UserManager<AppIdentityUser> userManager,
            IMapper mapper,
            FileHandling fileUploader)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _fileHandling = fileUploader;
        }

        public PersonalInformationInput PIInput { get; set; }
        public UpdatePasswordInput UPInput { get; set; }
        public UpdateEmailInput UEInput { get; set; }
        public UpdateUserNameInput UUInput { get; set; }
        public IFormFile? PFInput { get; set; }
        public IFormFile? RFInput { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                PIInput = _mapper.Map<PersonalInformationInput>(user);
            }
        }
        public async Task<IActionResult> OnPostAsync(PersonalInformationInput pIInput,IFormFile? pFInput, IFormFile? rFInput)
        {
            ModelState.Clear();
            if (!TryValidateModel(pIInput))
                return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            var profilePictureFileName = await _fileHandling.Uploadfile(pFInput, "ProfilePictures") ?? pIInput.ProfilePictureFileName;
            if (profilePictureFileName != pIInput.ProfilePictureFileName)
                await _fileHandling.DeleteFile("ProfilePictures", pIInput.ProfilePictureFileName);
            var resumeFileName = await _fileHandling.Uploadfile(rFInput, "ResumeFiles") ?? pIInput.ResumeFileName;
            if (profilePictureFileName != pIInput.ProfilePictureFileName)
                await _fileHandling.DeleteFile("ResumeFiles", pIInput.ResumeFileName);

            user.FirstName = pIInput.FirstName;
            user.MiddleName = pIInput.MiddleName;
            user.LastName = pIInput.LastName;
            user.DateOfBirth = pIInput.DateOfBirth;
            user.PhoneNumber = pIInput.PhoneNumber;
            user.Website = pIInput.Website;
            user.Degree = pIInput.Degree;
            user.Address = pIInput.Address;
            user.ProfilePictureFileName = profilePictureFileName;
            user.ResumeFileName = resumeFileName;
            var result = await _userManager.UpdateAsync(user);
            TempData["alert-message"] = "Profile Updated Successfully";
            if (!result.Succeeded)
                TempData["alert-message"] = result.Errors.First().Description;
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUpdatePassword(UpdatePasswordInput uPInput)
        {
            ModelState.Clear();
            if (!TryValidateModel(uPInput))
                return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            var result = await _userManager.ChangePasswordAsync(user, uPInput.OldPassword, uPInput.NewPassword);
            TempData["alert-message"] = "Password Updated Successfully";
            if (!result.Succeeded)
                TempData["alert-message"] = result.Errors.First().Description;
            await _signInManager.SignOutAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUpdateUserName(UpdateUserNameInput uUInput)
        {
            ModelState.Clear();
            if (!TryValidateModel(uUInput))
                return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            user.UserName = uUInput.UserName;
            var result = await _userManager.UpdateAsync(user);
            TempData["alert-message"] = "User Name Updated Successfully";
            if (!result.Succeeded)
                TempData["alert-message"] = result.Errors.First().Description;
            await _signInManager.SignOutAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostUpdateEmail(UpdateEmailInput uEInput)
        {
            ModelState.Clear();
            if (!TryValidateModel(uEInput))
                return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            user.Email = uEInput.Email;
            var result = await _userManager.UpdateAsync(user);
            TempData["alert-message"] = "Email Updated Successfully";
            if (!result.Succeeded)
                TempData["alert-message"] = result.Errors.First().Description;
            await _signInManager.SignOutAsync();
            return RedirectToPage();
        }

    }
}
