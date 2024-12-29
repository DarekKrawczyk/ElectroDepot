using System.Net.Http;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

public static class LinkValidator
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<ValidationResult> ValidateDatasheetLinkAsync(string name, ValidationContext context)
    {
        // If input is null or empty, return ValidationResult.Success
        if (string.IsNullOrEmpty(name))
        {
            return ValidationResult.Success;
        }

        // Check if the URL is valid
        bool isValid = Uri.TryCreate(name, UriKind.Absolute, out Uri? uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (!isValid)
        {
            return new ValidationResult("Link is invalid");
        }

        try
        {
            // Send a GET request to check if the page can be opened
            var response = await _httpClient.GetAsync(uriResult);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                // If the page content is not empty, return ValidationResult.Success
                string content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Link does not contain any content");
                }
            }
            else
            {
                return new ValidationResult("Unable to access the page or it returned an error");
            }
        }
        catch (Exception)
        {
            return new ValidationResult("An error occurred while trying to access the page");
        }
    }
}
