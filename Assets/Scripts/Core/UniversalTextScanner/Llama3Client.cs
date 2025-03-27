    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    namespace UniversalText.Core
    {
        /// <summary>
        /// Handles communication with the self-hosted Llama 3 instance via Ollama
        /// </summary>
        public class Llama3Client
        {
            private readonly string _baseUrl;
            private readonly string _modelName;

            // Constructor: Initializes the base URL for the Llama 3 instance
            public Llama3Client(string baseUrl, string modelName = "llama3")
            {
                _baseUrl = baseUrl;
                _modelName = modelName;
            }

            /// <summary>
            /// Sends the raw RTR to Llama 3 and retrieves the enhanced version
            /// </summary>
            public async Task<string> GetEnhancedDescriptionAsync(string rawRtr)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Create Ollama-compatible request
                        var requestData = new OllamaRequest
                        {
                            model = _modelName,
                            prompt = $"You are an AI assistant that improves text descriptions for virtual environments. Make this description more natural and coherent while preserving all spatial information: {rawRtr}",
                            stream = false
                        };

                        // Convert the request data to JSON
                        string json = JsonUtility.ToJson(requestData);
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                        Debug.Log($"Sending request to Ollama at {_baseUrl}/api/generate");
                        
                        // Send POST request to Ollama endpoint
                        HttpResponseMessage response = await client.PostAsync($"{_baseUrl}/api/generate", content);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the Ollama response
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Debug.Log($"Received response from Ollama: {responseBody}");
                            
                            OllamaResponse responseData = JsonUtility.FromJson<OllamaResponse>(responseBody);
                            return responseData.response;
                        }
                        else
                        {
                            Debug.LogError($"Llama 3 request failed: {response.StatusCode}");
                            return rawRtr;  // Return raw RTR if Llama 3 fails
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Exception in Llama 3 client: {ex.Message}");
                        return rawRtr;  // Return raw RTR on exception
                    }
                }
            }
        }

        // Helper classes for Ollama API serialization
        [Serializable]
        public class OllamaRequest
        {
            public string model;
            public string prompt;
            public bool stream;
        }

        [Serializable]
        public class OllamaResponse
        {
            public string model;
            public string response;
        }
    }   