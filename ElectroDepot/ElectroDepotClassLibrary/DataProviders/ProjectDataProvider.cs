using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using System.Text.Json;
using System.Text;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Utility;
using Avalonia.Media.Imaging;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class ProjectDataProvider : BaseDataProvider
    {
        public ProjectDataProvider(string url) : base(url) { }
        #region API Calls
        public async Task<Project> CreateProject(Project project)
        {
            var json = JsonSerializer.Serialize(project.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ProjectEndpoints.Create();
            var response = await HTTPClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var resjson = await response.Content.ReadAsStringAsync();
                ProjectDTO projectWithID = JsonSerializer.Deserialize<ProjectDTO>(resjson, options);

                return projectWithID.ToModel();
            }
            else
            {
                return null;
            }
        }
        
        public async Task<Project> GetProjectOfProjectComponent(ProjectComponent projectComponent)
        {
            try
            {
                string url = ProjectEndpoints.GetProjectOfProjectComponent(projectComponent.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    ProjectDTO projectWithID = JsonSerializer.Deserialize<ProjectDTO>(json, options);

                    return projectWithID.ToModel();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Project> GetProjectByID(int ProjectID)
        {
            try
            {
                string url = ProjectEndpoints.GetByID(ProjectID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    ProjectDTO projectWithID= JsonSerializer.Deserialize<ProjectDTO>(json, options);

                    return projectWithID.ToModel();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<double> GetProjectPrice(Project project)
        {
            try
            {
                string url = ProjectEndpoints.GetPriceByID(project.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    double price = JsonSerializer.Deserialize<double>(json, options);
                    return price;
                }
                else
                {
                    return -1.0;
                }
            }
            catch (Exception ex)
            {
                return -1.0;
            }
        }

        public async Task<Bitmap> GetImageOfProjectByID(Project project)
        {
            byte[] image = new byte[] { };
            Bitmap bitmap = null;
            try
            {
                string url = ProjectEndpoints.GetImageOfProjectByID(project.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    image = JsonSerializer.Deserialize<byte[]>(json, options);

                    bitmap = ImageConverterUtility.BytesToBitmap(image);

                    return bitmap;
                }
                else
                {
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                return bitmap;
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            try
            {
                string url = ProjectEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProjectDTO> projects = JsonSerializer.Deserialize<IEnumerable<ProjectDTO>>(json, options);
                    List<Project> projectModels = projects.Select(x=>x.ToModel()).ToList();

                    return projectModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectOfUser(User user)
        {
            try
            {
                string url = ProjectEndpoints.GetAllOfUser(user.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProjectDTO> projectsOfUser = JsonSerializer.Deserialize<IEnumerable<ProjectDTO>>(json, options);
                    List<Project> projectModels = projectsOfUser.Select(x=>x.ToModel()).ToList();

                    return projectModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Component>> GetAllComponentsFromProject(Project project)
        {
            try
            {
                string url = ProjectEndpoints.GetAllComponentsFromProject(project.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ComponentDTO> componentsOfProject = JsonSerializer.Deserialize<IEnumerable<ComponentDTO>>(json, options);
                    return componentsOfProject.Select(x=>x.ToModel());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Project> UpdateProject(Project project)
        {
            var json = JsonSerializer.Serialize(project.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ProjectEndpoints.Update(project.ID);
            var response = await HTTPClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var jsonRes = await response.Content.ReadAsStringAsync();
                ProjectDTO projectWithID = JsonSerializer.Deserialize<ProjectDTO>(jsonRes, options);

                Project result = projectWithID.ToModel();
                result.Image = project.Image;
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteProject(Project project)
        {
            string url = ProjectEndpoints.Delete(project.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}