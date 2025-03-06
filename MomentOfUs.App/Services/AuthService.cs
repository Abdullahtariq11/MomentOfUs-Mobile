using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomentOfUs.App.Models;
using Newtonsoft.Json;

namespace MomentOfUs.App.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://192.168.243.230:7241/api/Users";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(string email, string password,bool rememberMe)
        {
            try
            {
                var loginRequest = new LoginModel
                {
                    Username = email,
                    Password = password,
                    RememberMe = rememberMe

                };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/Login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content; //JWT token
                }

                return string.Empty;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in: {ex.Message}");
                return string.Empty;
            }
        }
        public async Task<bool> LogoutAsync()
        {
            try
            {
                // Get token preference
                var token = Preferences.Get("AuthToken", string.Empty);
                if(string.IsNullOrEmpty(token))
                {
                    return false; //No token found user already logged out
                }
                // create request with token
                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Logout");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                   
                    Preferences.Remove("AuthToken");
                    return true; // Logout successful
                }

                return false; // Logout failed


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging out: {ex.Message}");
                return false;
            }
        }
    }
}
