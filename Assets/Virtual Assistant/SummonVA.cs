/*
HOW TO USE

This script contains two ways of summoning the virtual assistant.

1. By saying a keyword/target phrase
This is the code in the first comment. In order for this to work, place both the Whisper and MicMonitor prefabs into the scene.
This relies on having the whisper package installed.

Possible Customizations:
- Change detected keyword in parameter field of the script
- Alter which language model to use. Within the whisper folder, there is ggml-tiny and ggml-medium. ggml-medium is more 
  accurate at detecting words at the cost of being more resource intensive.

2. By pressing a button
This is the code in the second comment. For this, place OVRCameraRig into the scene.

Customizations:
- Change button detected in the code. See OVR documentation for possible inputs.
*/


using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;

/*
namespace Whisper.Samples
{
    /// Stream transcription from microphone input.
    public class StreamingSampleMic : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
    
        [Header("UI")] 
        private WhisperStream _stream;

        public string targetPhrase; // Target phrase to detect
        public AudioSource audioSource; // Reference to the AudioSource

        private async void Start()
        {
            _stream = await whisper.CreateStream(microphoneRecord);
            _stream.OnResultUpdated += OnResult;
            _stream.OnSegmentUpdated += OnSegmentUpdated;
            _stream.OnSegmentFinished += OnSegmentFinished;
            _stream.OnStreamFinished += OnFinished;

            ToggleListen();
        }

        private void ToggleListen()
        {
            if (!microphoneRecord.IsRecording)
            {
                _stream.StartStream();
                microphoneRecord.StartRecord();
            }
            else
                microphoneRecord.StopRecord();
        
        }

    
        private void OnResult(string result)
        {

            // Detect if phrase was said
            if (result.ToLower().Contains(targetPhrase.ToLower()))
            {
                Debug.Log($"Target phrase detected: {targetPhrase}");
                // Add summon virtual assistant here
                if (audioSource != null)
                {
                    audioSource.Play(); // Play the sound effect
                }
                else
                {
                    Debug.LogWarning("AudioSource is not assigned!");
                }
                ToggleListen();
                ToggleListen();
            }
        }
        
        private void OnSegmentUpdated(WhisperResult segment)
        {
            print($"Segment updated: {segment.Result}");
        }
        
        private void OnSegmentFinished(WhisperResult segment)
        {
            print($"Segment finished: {segment.Result}");
        }
        
        private void OnFinished(string finalResult)
        {
            print("Stream finished!");
        }
    }
}*/

/*
public class OculusButtonPress : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // Button to activate
        {
            Debug.Log("Button pressed!");
            // Summon virtual assistant here
            if (audioSource != null)
            {
                audioSource.Play(); 
                Debug.Log("Sound played!");
            }
            else
            {
                Debug.LogWarning("AudioSource is not assigned!");
            }
        }
    }
}*/