using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.Repositories.Contracts;

namespace MyPortfolio.Pages.AboutPage
{
    public class IndexModel : PageModel
    {
        private readonly IBaseRepository<Skill> _skillRepo;
        private readonly UserManager<AppIdentityUser> _userManager;
        public IndexModel(IBaseRepository<Skill> skillRepo,UserManager<AppIdentityUser> userManager)
        {
            _skillRepo = skillRepo;
            _userManager = userManager;
        }   
        public List<Skill> Skills { get; set; }
        public AppIdentityUser MyInformation { get; set; }
        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            MyInformation = users.First();
        }
        public async Task<JsonResult> OnGetSkills()
        {
            var skills = await _skillRepo.GetAll();
            skills = skills.OrderByDescending(x => x.SkillPercentage).ToList();
            var filteredSkills = new
            {
                NativeSkills = skills.Where(x => x.SkillCategory == SkillCategories.Native),
                FrameworkSkills = skills.Where(x => x.SkillCategory == SkillCategories.Framework)
            };
            return new JsonResult(filteredSkills);
        }
    }
}
