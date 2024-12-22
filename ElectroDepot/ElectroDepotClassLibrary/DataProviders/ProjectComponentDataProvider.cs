using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Endpoints;
using System.Text.Json;
using System.Text;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.DataProviders
{
    public class ProjectComponentDataProvider : BaseDataProvider
    {
        public ProjectComponentDataProvider(string url) : base(url) { }
        #region API Calls
        public async Task<bool> CreateProjectComponent(ProjectComponent ProjectComponent)
        {
            var json = JsonSerializer.Serialize(ProjectComponent.ToCreateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ProjectComponentEndpoints.Create();
            var response = await HTTPClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ProjectComponent>> GetAllProjectComponents()
        {
            try
            {
                string url = ProjectComponentEndpoints.GetAll();
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProjectComponentDTO> projects = JsonSerializer.Deserialize<IEnumerable<ProjectComponentDTO>>(json, options);

                    return projects.Select(x=>x.ToModel()).ToList();
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

        public async Task<IEnumerable<ProjectComponent>> GetAllProjectComponentsOfProject(Project project)
        {
            try
            {
                string url = ProjectComponentEndpoints.GetAllByProject(project.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProjectComponentDTO> projectsOfUser = JsonSerializer.Deserialize<IEnumerable<ProjectComponentDTO>>(json, options);

                    return projectsOfUser.Select(x=>x.ToModel()).ToList();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectComponent>> GetAllProjectComponentsOfComponents(Component component)
        {
            try
            {
                string url = ProjectComponentEndpoints.GetAllProjectComponentsOfComponents(component.ID);
                var response = await HTTPClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var json = await response.Content.ReadAsStringAsync();
                    IEnumerable<ProjectComponentDTO> projectsOfUser = JsonSerializer.Deserialize<IEnumerable<ProjectComponentDTO>>(json, options);

                    return projectsOfUser.Select(x => x.ToModel()).ToList();
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

        public async Task<bool> UpdateProjectComponent(ProjectComponent ProjectComponent)
        {
            var json = JsonSerializer.Serialize(ProjectComponent.ToUpdateDTO());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ProjectComponentEndpoints.Update(ProjectComponent.ID);
            var response = await HTTPClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProjectComponent(ProjectComponent ProjectComponent)
        {
            string url = ProjectComponentEndpoints.Delete(ProjectComponent.ID);
            var response = await HTTPClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
