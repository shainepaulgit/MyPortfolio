using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.ViewModels;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace MyPortfolio.Pages.ViewComponents
{
    public class SidebarViewComponent:ViewComponent
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public SidebarViewComponent(UserManager<AppIdentityUser> userManager,SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var menus = new List<SidebarViewModel>
            {
                new SidebarViewModel { Icon = "fa fa-house", MenuName = "Home", MenuUrl="/Index"},
                new SidebarViewModel { Icon = "fa fa-user", MenuName = "About", MenuUrl="/AboutPage/Index" },
                new SidebarViewModel { Icon = "fa fa-screwdriver-wrench", MenuName="Services",MenuUrl = "/ServicesPage/Index"},
                new SidebarViewModel { Icon = "fa fa-phone", MenuName = "Contact", MenuUrl= "/ContactPage/Index"},
                new SidebarViewModel {Icon = "fa fa-user-gear",MenuName = "Admin",MenuUrl = "/AdminPage/Index"},
                new SidebarViewModel {Icon = "fa-brands fa-github",MenuName = "GitHubRequest",MenuUrl = "/GitHubRequestPage/Index"},
                new SidebarViewModel {Icon = "fa fa-user",MenuName = "Profile",MenuUrl = "/ProfilePage/Index"},
                

            };
            if (!_signInManager.IsSignedIn((ClaimsPrincipal)User) || !User.IsInRole("Admin"))
            {
                menus = menus.Where(x =>
                    x.MenuName != "Admin" &&
                    x.MenuName != "GitHubRequest" &&
                    x.MenuName != "Profile"
                ).ToList();
            }
            return View(menus);
        }
    }
}
