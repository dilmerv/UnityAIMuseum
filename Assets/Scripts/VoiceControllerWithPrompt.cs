using DilmerGames.Core.Singletons;
using Meta.WitAi;
using Oculus.Voice;
using OpenAI.Images;
using TMPro;
using UnityEngine;

public class VoiceControllerWithPrompt : Singleton<VoiceControllerWithPrompt>
{

    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI partialTranscriptText;

    private Transform objectToActOnWithVoice;

    private void Awake()
    {
        partialTranscriptText.text = string.Empty;

        appVoiceExperience.events.OnFullTranscription.AddListener(OnFullTranscription);
        appVoiceExperience.events.onPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.events.OnRequestCreated.AddListener(OnRequestCreated);
        appVoiceExperience.events.OnRequestCompleted.AddListener(OnRequestCompleted);
    }

    private void OnFullTranscription(string transcript)
    {
        var progress = objectToActOnWithVoice.parent.GetComponentInChildren<Progress>();

        if (progress != null)
        {
            progress.StartProgress("Generating AI Image");
        }

        ImageGenerator.Instance.GenerateImage(transcript, objectToActOnWithVoice, ImageSize.Small, OnImageGenerated);

        var frame = objectToActOnWithVoice.parent.GetComponentInChildren<Frame>();

        if (frame != null)
        {
            frame.FramePrompt.text = transcript;
        }
    }

    private void OnImageGenerated(Transform transformTarget, Texture2D texture)
    {
        var progress = objectToActOnWithVoice.parent.GetComponentInChildren<Progress>();

        if (progress != null)
        {
            progress.StopProgress();
        }

        var frame = objectToActOnWithVoice.parent.GetComponentInChildren<Frame>();

        if (frame != null)
        {
            frame.UpdateTexture(texture);
        }
    }

    private void OnPartialTranscription(string transcript)
    {
        partialTranscriptText.text = transcript;
        var frame = objectToActOnWithVoice.parent.GetComponentInChildren<Frame>();

        if (frame != null)
        {
            frame.FramePrompt.text = transcript;
        }
    }

    private void OnRequestCreated(WitRequest request)
    {
        Logger.Instance.LogInfo("OnRequestCreated Activated");
    }

    private void OnRequestCompleted()
    {
        Logger.Instance.LogInfo("OnRequestCompleted Deactivated");
    }

    public void ActivateVoice(Transform selectedTransform)
    {
        objectToActOnWithVoice = selectedTransform;
        Logger.Instance.LogInfo("Update is about to activate voice");
        appVoiceExperience.Activate();
    }
}
