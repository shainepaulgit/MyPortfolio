using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Repositories.Contracts;
using System.Runtime.CompilerServices;

namespace MyPortfolio.Pages.ViewComponents
{
    public class FormManagementModalViewComponent:ViewComponent
    {
        private readonly IBaseRepository<Skill> _skillRepo;
        private readonly IBaseRepository<ServiceCategory> _serviceCategoryRepo;
        private readonly IBaseRepository<Project> _projectRepo;
        private readonly IBaseRepository<ProjectCategory> _projectCategoryRepo;
        private readonly IMapper _mapper;
        public FormManagementModalViewComponent(
            IBaseRepository<Skill> skillRepo,
            IBaseRepository<ServiceCategory> serviceCategoryRepo,
            IBaseRepository<Project> projectRepo,
            IBaseRepository<ProjectCategory> projectCategoryRepo,
            IMapper mapper
            )
        {
            _skillRepo = skillRepo;
            _serviceCategoryRepo = serviceCategoryRepo;
            _projectRepo = projectRepo;
            _projectCategoryRepo = projectCategoryRepo;
            _mapper = mapper;

        }
        public async Task<IViewComponentResult> InvokeAsync(string view, string id)
        {
            switch (view)
            {
                case "SkillForm":
                    var skill = await _skillRepo.GetOne(id);
                    var skillInput = _mapper.Map<SkillInput>(skill);
                    if(skillInput == null)
                        skillInput = new SkillInput();
                    
                    return View(view, skillInput);
                case "ServiceCategoryForm":
                    var serviceCategory = await _serviceCategoryRepo.GetOne(id);
                    var serviceCategoryInput = _mapper.Map<ServiceCategoryInput>(serviceCategory);
                    if (serviceCategoryInput == null)
                        serviceCategoryInput = new ServiceCategoryInput();
                    return View(view, serviceCategoryInput);
                case "ProjectForm":
                    var project = await _projectRepo.GetOne(id);
                    var projectInput = _mapper.Map<ProjectInput>(project);
                    if (projectInput == null)
                        projectInput = new ProjectInput();
                    return View(view, projectInput);
                case "ProjectCategoryForm":
                    var projectCategory = await _projectCategoryRepo.GetOne(id);
                    var projectCategoryInput = _mapper.Map<ProjectCategoryInput>(projectCategory);
                    if (projectCategoryInput == null)
                        projectCategoryInput = new ProjectCategoryInput();
                    return View(view, projectCategoryInput);
                default:
                    return Content("Invalid view requested.");
            }
        }
    }
}
