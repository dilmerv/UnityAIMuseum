using Oculus.Voice;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class VoiceController : MonoBehaviour
{
    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI partialTranscriptText;

    private bool appVoiceActive;

    private void Awake()
    {
        partialTranscriptText.text = string.Empty;

        appVoiceExperience.events.OnFullTranscription.AddListener((transcript) =>
        {
            OpenAITests.Instance.GenerateImage(transcript);
        });

        appVoiceExperience.events.onPartialTranscription.AddListener((transcript) =>
        {
            partialTranscriptText.text = transcript;
        });

        appVoiceExperience.events.OnRequestCreated.AddListener((request) =>
        {
            appVoiceActive = true;
            Logger.Instance.LogInfo("OnRequestCreated Activated");
        });

        appVoiceExperience.events.OnRequestCompleted.AddListener(() =>
        {
            appVoiceActive = false;
            Logger.Instance.LogInfo("OnRequestCompleted Deactivated");
        });
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !appVoiceActive)
        {
            Logger.Instance.LogInfo("Update is about to activate voice");
            appVoiceExperience.Activate();
        }
    }
}


