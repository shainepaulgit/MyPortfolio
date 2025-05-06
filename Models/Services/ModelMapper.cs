using AutoMapper;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.InputModels;

namespace MyPortfolio.Models.Services
{
    public class ModelMapper:Profile
    {
        public ModelMapper()
        {
            CreateMap<SkillInput,Skill>().ReverseMap();
            CreateMap<ServiceCategoryInput, ServiceCategory>().ReverseMap();
            CreateMap<ProjectInput, Project>().ReverseMap();
            CreateMap<ProjectCategoryInput, ProjectCategory>().ReverseMap();
            CreateMap<PersonalInformationInput, AppIdentityUser>().ReverseMap();
            CreateMap<GitHubRequestInput, GitHubRequest>().ReverseMap();
        }
    }
}
