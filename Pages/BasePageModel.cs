using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.Repositories.Contracts;

namespace MyPortfolio.Pages
{
    public class BasePageModel<T, T2> : PageModel
        where T : BaseEntity
    {
        private readonly IBaseRepository<T> _repo;
        private readonly IMapper _mapper;
        public BasePageModel(IBaseRepository<T> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public virtual async Task<IActionResult> OnPostAsync(T2 input)
        {
            if (!TryValidateModel(input))
                return BadRequest(ModelState);
            var entity = _mapper.Map<T>(input);
            if (entity.Id != 0)
                await _repo.Update(entity, entity.Id.ToString());
            else
                await _repo.Add(entity);
            return RedirectToPage();
        }
        public virtual async Task<IActionResult> OnGetDeleteAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest("Id is required.");
            await _repo.Delete(id);
            return RedirectToPage();
        }
    }
}
