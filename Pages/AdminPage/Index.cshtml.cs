using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MyPortfolio.Models.DTOs;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;
using MyPortfolio.Models.Repositories.Contracts;
using MyPortfolio.Models.Services;
using System.ComponentModel.DataAnnotations;


namespace MyPortfolio.Pages.AdminPage
{
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IBaseRepository<Skill> _skillRepo;
        private readonly IBaseRepository<Project> _projectRepo;
        private readonly IBaseRepository<ProjectCategory> _projectCategoryRepo;
        private readonly IBaseRepository<ServiceCategory> _serviceCategoryRepo;
        private readonly IBaseRepository<Viewer> _viewerRepo;
        private readonly IBaseRepository<GitHubRequest> _gitHubRequestRepo;
        private readonly IMapper _mapper;
        private readonly FileHandling _fileHandling;
        public IndexModel(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager,
            IBaseRepository<Skill> skillRepo,
            IBaseRepository<Project> projectRepo,
            IBaseRepository<ProjectCategory> projectCategoryRepo,
            IBaseRepository<ServiceCategory> serviceCategoryRepo,
            IBaseRepository<Viewer> viewerRepo,
            IBaseRepository<GitHubRequest> gitHubRequestRepo,
            IMapper mapper,
            FileHandling fileHandling)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _skillRepo = skillRepo;
            _projectRepo = projectRepo;
            _projectCategoryRepo = projectCategoryRepo;
            _serviceCategoryRepo = serviceCategoryRepo;
            _viewerRepo = viewerRepo;
            _gitHubRequestRepo = gitHubRequestRepo;
            _mapper = mapper;
            _fileHandling = fileHandling;

        }
        [BindProperty]
        public SkillInput SkillInput { get; set; }
        [BindProperty]
        public ServiceCategoryInput ServiceCategoryInput { get; set; }
        [BindProperty]
        public ProjectInput ProjectInput { get; set; }
        [BindProperty]
        public ProjectCategoryInput ProjectCategoryInput { get; set; }
        public int ViewerTodayCount { get; set; }


        public async Task OnGetAsync()
        {
            var viewers = await _viewerRepo.GetAll();
            ViewerTodayCount = viewers.Where(x => x.CreatedAt.Date == DateTime.Now.Date).Count();
        }

        public async Task<JsonResult> OnGetDataTable()
        {
            var projectCategories = await _projectCategoryRepo.GetAll();
            var project = await _projectRepo.GetAll();
            projectCategories = projectCategories
                .Select(r => new ProjectCategory
                {
                    Id = r.Id,
                    IconName = r.IconName,
                    Title = r.Title,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList();
            project = project
                .Select(r => new Project
                {
                    Id= r.Id,
                    ProjectTitle = r.ProjectTitle,
                    ProjectDescription = r.ProjectDescription,
                    ProjectPictureFileName = r.ProjectPictureFileName,
                    RedirectUrl = r.RedirectUrl,
                    IsGitHubRepository = r.IsGitHubRepository,
                    ProjectCategoryId = r.ProjectCategoryId,

                }).ToList();

               

            var skillls = await _skillRepo.GetAll();
            var dataTableObj = new DataTable
            {
                Skills = await _skillRepo.GetAll(),
                Projects = project,
                ProjectCategories = projectCategories,
                ServiceCategories = await _serviceCategoryRepo.GetAll()

            };
            return new JsonResult(dataTableObj);
        }

        public async Task<JsonResult> OnGetGitHubRequestsStatus()
        {
            var requests = await _gitHubRequestRepo.GetAll();
            var requestStatus = new
            {
                TotalRequests = requests.Count(),
                PendingRequests = requests.Where(x => x.Status == GitHubRequestStatus.Pending).Count(),
                AcceptedRequests = requests.Where(x => x.Status == GitHubRequestStatus.Approved).Count(),
                DeclinedRequests = requests.Where(x => x.Status == GitHubRequestStatus.Declined).Count()
            };
            return new JsonResult(requestStatus);
        }
        public async Task<JsonResult> OnGetViewersPerMonth()
        {
            var viewers = await _viewerRepo.GetAll();
            var currentYear = DateTime.UtcNow.AddHours(8).Year;
            var viewersPerMonth = new
            {
                January = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 1).Count().ToString(),
                February = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 2).Count().ToString(),
                March = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 3).Count().ToString(),
                April = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 4).Count().ToString(),
                May = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 5).Count().ToString(),
                June = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 6).Count().ToString(),
                July = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 7).Count().ToString(),
                August = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 8).Count().ToString(),
                September = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 9).Count().ToString(),
                October = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 10).Count().ToString(),
                November = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 11).Count().ToString(),
                December = viewers.Where(x => x.CreatedAt.Year == currentYear && x.CreatedAt.Month == 12).Count().ToString()
            };
            return new JsonResult(viewersPerMonth);
        }
        public async Task<IActionResult> OnGetLoadViewComponentSubmitAsync(string view, string id)
        {
            return ViewComponent("FormManagementModal", new {view, id });
        }

        public async Task<IActionResult> OnPostAsync(string type,IFormFile? pictureFile)
        {
            ModelState.Clear();
            string fileName;
            switch (type)
            {
                case "skill":
                    fileName = await _fileHandling.Uploadfile(pictureFile, "LogoPictureFiles");
                    if (fileName != null)
                    {
                       
                        await _fileHandling.DeleteFile("LogoPictureFiles", SkillInput.LogoFileName);
                        SkillInput.LogoFileName = fileName;

                    }
                    return await OnPostAsync(SkillInput, type, pictureFile);
                case "project":
                    fileName = await _fileHandling.Uploadfile(pictureFile, "ProjectPictureFiles");
                    if (fileName != null)
                    {
                      
                        await _fileHandling.DeleteFile("ProjectPictureFiles", ProjectInput.ProjectPictureFileName);
                        ProjectInput.ProjectPictureFileName = fileName;

                    }
                    return await OnPostAsync(ProjectInput,type,pictureFile);
                case "projectCategory":
                    return await OnPostAsync(ProjectCategoryInput,type,pictureFile);
                case "serviceCategory":
                    return await OnPostAsync(ServiceCategoryInput,type,pictureFile);
                default:
                    return BadRequest("Invalid entity type.");
            }


        }
        public async Task<IActionResult> OnPostAsync<T>(T input,string type, IFormFile? pictureFile) 
            where T: BaseInput
        {
            if (!TryValidateModel(input))
                return BadRequest(ModelState);
            var isNew = input.Id == null;
            var alertMessage = isNew ? "Successfully Added" : "Successfully Updated";

            switch (type)
            {
                case "skill":
                    var skill = _mapper.Map<Skill>(input);
                    if (isNew)
                        await _skillRepo.Add(skill);
                    else
                        await _skillRepo.Update(skill, skill.Id.ToString());
                    break;

                case "project":
                    var project = _mapper.Map<Project>(input);
                    if (isNew)
                        await _projectRepo.Add(project);
                    else
                        await _projectRepo.Update(project, project.Id.ToString());
                    break;

                case "projectCategory":
                    var projectCategory = _mapper.Map<ProjectCategory>(input);
                    if (isNew)
                        await _projectCategoryRepo.Add(projectCategory);
                    else
                        await _projectCategoryRepo.Update(projectCategory, projectCategory.Id.ToString());
                    break;

                case "serviceCategory":
                    var serviceCategory = _mapper.Map<ServiceCategory>(input);
                    if (isNew)
                        await _serviceCategoryRepo.Add(serviceCategory);
                    else
                        await _serviceCategoryRepo.Update(serviceCategory, serviceCategory.Id.ToString());
                    break;

                default:
                    return BadRequest("Invalid entity type.");
            }

            TempData["alert-message"] = alertMessage;
            return RedirectToPage();
        }
        public virtual async Task<IActionResult> OnPostDelete(string recordId,string type)
        {
            if (string.IsNullOrWhiteSpace(recordId))
                return BadRequest("Invalid record ID.");

            switch (type)
            {
                case "skill":
                    var skill = await _skillRepo.GetOne(recordId);
                    await _fileHandling.DeleteFile("LogoPictureFiles", skill.LogoFileName);
                    await _skillRepo.Delete(recordId);
                    break;
                case "project":
                    var project = await _projectRepo.GetOne(recordId);
                    await _fileHandling.DeleteFile("ProjectPictureFiles", project.ProjectPictureFileName);
                    await _projectRepo.Delete(recordId);
                    break;
                case "serviceCategory":
                    await _serviceCategoryRepo.Delete(recordId);
                    break;
                case "projectCategory":
                    await _projectCategoryRepo.Delete(recordId);
                    break;
            }
            return RedirectToPage();
        }
      


    }
}
