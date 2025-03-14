using System.Net.Http;
using System.Text;
using UnityEngine;

namespace UniversalText.Core
{
    /// 
    /// Handles communication with the self-hosted Llama 3 instance
    /// 
    public class Llama3Client
    {
        private readonly string _baseUrl;

        // Constructor: Initializes the base URL for the Llama 3 instance
        public Llama3Client(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// 
        /// Sends the raw RTR to Llama 3 and retrieves the enhanced version
        /// 
        public async System.Threading.Tasks.Task<string> GetEnhancedDescriptionAsync(string rawRtr)
        {
            using (HttpClient client = new HttpClient())
            {
                // Formulate the request payload
                var requestData = new
                {
                    prompt = $"Make this description more natural and coherent: {rawRtr}"
                };

                // Convert the request data to JSON
                string json = JsonUtility.ToJson(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to Llama 3 endpoint
                HttpResponseMessage response = await client.PostAsync($"{_baseUrl}/generate", content);
                if (response.IsSuccessStatusCode)
                {
                    // Return the enhanced description
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    Debug.LogError($"Llama 3 request failed: {response.StatusCode}");
                    return rawRtr;  // Return raw RTR if Llama 3 fails
                }
            }
        }
    }
}
