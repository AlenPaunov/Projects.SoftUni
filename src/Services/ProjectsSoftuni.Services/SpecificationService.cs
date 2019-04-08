using CloudinaryDotNet;
using ProjectsSoftuni.Services.Common;
using ProjectsSoftuni.Services.Models.InputModels;
using System;
using System.Threading.Tasks;
using ProjectsSoftuni.Services.Models.Projects;

namespace ProjectsSoftuni.Services
{

    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Contracts;

    public class SpecificationService : ISpecificationService
    {
        private readonly IRepository<Specification> specificationsRepository;
        private readonly Cloudinary cloudinary;
        private readonly IProjectService projectService;

        public SpecificationService(
            IRepository<Specification> specificationsRepository,
            Cloudinary cloudinary,
            IProjectService projectService)
        {
            this.specificationsRepository = specificationsRepository;
            this.cloudinary = cloudinary;
            this.projectService = projectService;
        }

        public async Task<bool> UploadSpecificationAsync(UploadSpecificationsInputModel model)
        {
            var projectModel = await this.projectService.GetProjectByIdAsync<SpecificationProjectViewModel>(model.ProjectId);

            if (projectModel == null)
            {
                return false;
            }

            var fileName = this.CreateFileName(projectModel.Name);

            var fileUrl =
                await ApplicationCloudinary.UploadFile(this.cloudinary, model.SpecificationsFile, fileName);

            var specificationUrlId = this.GetSpecificationIdFromUlr(fileUrl);

            var specification = new Specification()
            {
                SpecificationId = specificationUrlId,
            };

            var specificationId = await this.projectService.UpdateSpecificationAsync(specification, model.ProjectId);

            return true;
        }

        private string CreateFileName(string projectName)
        {
            string res = string.Format(ServicesConstants.SpecificationNameTemplate, projectName);
            return res;
        }

        private string GetSpecificationIdFromUlr(string url)
        {
            var tokens = url.Split('/');

            var fileId = tokens[tokens.Length - 2];
            var fileName = tokens[tokens.Length - 1];

            return fileId + "/" + fileName;
        }
    }
}
