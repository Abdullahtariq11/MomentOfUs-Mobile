using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using MomentOfUs.App.Models;
using System.Text.Json;
using System.Diagnostics;
using MomentOfUs.App.Dtos;

namespace MomentOfUs.App.Services
{
    public class JournalService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://192.168.183.118:885/api/Journals";

        // Cache JsonSerializerOptions to avoid CA1869
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public JournalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get all journals for the current user
        /// </summary>
        public async Task<List<JournalModel>> GetJournalsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Preferences.Get("AuthToken", string.Empty));

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Failed to load journals. StatusCode: {response.StatusCode}");
                    return new List<JournalModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<JournalModel>>(json, JsonOptions) ?? new List<JournalModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error fetching journals: {ex.Message}");
                return new List<JournalModel>();
            }
        }

        /// <summary>
        /// Get all shared journals for the user
        /// </summary>
        public async Task<List<JournalSharedModel>> GetSharedJournalsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/shared-with-me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Preferences.Get("AuthToken", string.Empty));
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Failed to load shared journals. StatusCode: {response.StatusCode}");
                    return new List<JournalSharedModel>();
                }
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<JournalSharedModel>>(json, JsonOptions) ?? new List<JournalSharedModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error fetching shared journals: {ex.Message}");
                return new List<JournalSharedModel>();
            }
        }

        /// <summary>
        /// Get Journal By Id
        /// </summary>
        public async Task<JournalModel?> GetJournalByIdAsync(string id)
        {
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("⚠️ No auth token found.");
                    return null;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Failed to load journal. StatusCode: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<JournalModel>(json, JsonOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error fetching journal: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Fetch journal entries by JournalId
        /// </summary>
        public async Task<List<JournalEntryModel>> GetJournalEntriesAsync(string journalId)
        {
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("⚠️ No auth token found.");
                    return new List<JournalEntryModel>();
                }

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{journalId}/entries");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Failed to load journal entries. StatusCode: {response.StatusCode}");
                    return new List<JournalEntryModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<JournalEntryModel>>(json, JsonOptions)
                       ?? new List<JournalEntryModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error fetching journal entries: {ex.Message}");
                return new List<JournalEntryModel>();
            }
        }
        ///<summary>
        /// Delete Journal Entry using JournalId and JournalEntryId
        /// </summary>
        /// 
        public async Task<bool> DeleteJournalEntry(string journalId, string journalEntryId)
        {
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("⚠️ No auth token found.");
                    return false;
                }

                var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/{journalId}/entries/{journalEntryId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Failed to delete entry. StatusCode: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception in DeleteJournalEntry: {ex.Message}");
                return false;
            }
        }

        public async Task<string> UpdateJournalEntryAsync(string journalId, string entryId, JournalEntryUpdateDto updateDto)
        {
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("⚠️ No auth token found.");
                    return "Unauthorized: No token found.";
                }

                var json = JsonSerializer.Serialize(updateDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/{journalId}/entries/{entryId}")
                {
                    Content = content
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Failed to update entry. StatusCode: {response.StatusCode}");
                    return $"Failed to update entry: {response.StatusCode}";
                }

                return "Success";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception in UpdateJournalEntryAsync: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> CreateJournalEntryAsync(string journalId, JournalEntryUpdateDto updateDto)
        {
            try
            {
                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("⚠️ No auth token found.");
                    return "Unauthorized: No token found.";
                }

                var json = JsonSerializer.Serialize(updateDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{journalId}/entries")
                {
                    Content = content
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"❌ Failed to update entry. StatusCode: {response.StatusCode}");
                    return $"Failed to update entry: {response.StatusCode}";
                }

                return "Success";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Exception in UpdateJournalEntryAsync: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }



    }

}
