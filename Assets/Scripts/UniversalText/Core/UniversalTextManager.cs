using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;
using System;
using UniversalText.UI;
using System.Threading.Tasks;


namespace UniversalText
{
    /// <summary>
    /// Manages the functionality of the Universal Text package - Should be
    /// attached to some persistent GameObject in a scene, such as the OVR Camera Rig.
    /// </summary>
    public class UniversalTextManager : MonoSingleton<UniversalTextManager>
    {
        [SerializeReference] public List<SearchPointConfig> searchPointConfigs = new List<SearchPointConfig>();

        [Header("Description Generation")]
        [SerializeField] private float _generateDelay = 1f;

        /// <summary>
        /// Fired when raw real-time text representation is generated, along with the result
        /// Args: (result, request timestamp)
        /// </summary>
        public event Action<string, System.DateTime> RTRGenerated;
        private float _sinceLastGenerate = 0f;


        // TODO: Llama 3 enhancement to be moved to a standalone class, temporarily resides in UTM 
        // --temp--
        [Header("Llama 3 Settings")]
        [SerializeField] private bool useLlama3Enhancement = false;
        [SerializeField] private string llama3BaseUrl = "http://localhost:11434"; // Updated to Ollama's default port

        private Llama3Client _llama3Client = null;
        /// <summary>
        /// Fired when enhanced real-time text representation is generated, along with the result
        /// Args: (result, request timestamp)
        /// </summary>
        public event Action<string, System.DateTime> EnhancedRTRGenerated;
        // --temp--\

        void Start()
        {
            InitSearchPoints();

            // --temp--
            // Set Llama3 base URL if using enhancement
            if (useLlama3Enhancement)
            {
                _llama3Client = new Llama3Client(llama3BaseUrl);
                RTRGenerated += (rtr, _) => {
                    StartCoroutine(GenerateEnhancedRTR(rtr));
                };
            }
            // --temp--\
        }

        private void Update()
        {
            _sinceLastGenerate += Time.deltaTime;
            if (_sinceLastGenerate > _generateDelay)
            {
                _sinceLastGenerate -= _generateDelay;
                System.DateTime generateCallTimestamp = System.DateTime.Now;
                RTRGenerated.Invoke(UniversalTextScanner.Instance.Generate(), generateCallTimestamp);
            }
        }

        private void InitSearchPoints()
        {
            foreach (SearchPointConfig searchPointConfig in searchPointConfigs)
            {
                UniversalTextScanner.Instance.AddSearchPoint(searchPointConfig.CreateSearchPoint());
            }
        }

        // --temp--
        /// <summary>
        /// Coroutine that prints an enhanced version of the provided real-time text representation
        /// </summary>
        private IEnumerator GenerateEnhancedRTR(string rtr)
        {
            if (string.IsNullOrEmpty(rtr))
            {
                yield break;
            }

            // TODO: fix latency by only processing most recent rtr instead of every single one

            // Use Llama 3 to enhance the description
            Task<string> enhancedTextTask = _llama3Client.GetEnhancedDescriptionAsync(rtr);
            System.DateTime requestTimestamp = System.DateTime.Now;
            yield return new WaitUntil(() => enhancedTextTask.IsCompleted);
            if (enhancedTextTask.Exception != null)
            {
                Debug.LogError($"Error generating enhanced text: {enhancedTextTask.Exception}");
                Debug.Log(UniversalTextScanner.Instance.Generate()); // Fallback to raw output
            }
            else
            {
                EnhancedRTRGenerated.Invoke(enhancedTextTask.Result, requestTimestamp);
            }
        }
        // --temp--\
    }
}
