using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Pages.ViewComponents
{
    public class ContentHeaderViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string headerText)
        {
            return View(headerText);
        }
    }
}
