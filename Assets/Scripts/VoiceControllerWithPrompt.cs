using DilmerGames.Core.Singletons;
using Oculus.Voice;
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

    private bool appVoiceActive;

    private Transform objectToActOnWithVoice;

    private void Awake()
    {
        partialTranscriptText.text = string.Empty;

        appVoiceExperience.events.OnFullTranscription.AddListener((transcript) =>
        {
            var progress = objectToActOnWithVoice.parent?.GetComponentInChildren<Progress>();
            if(progress != null)
                progress.StartProgress("Generating AI Image");

            MuseumImageGenerator.Instance.GenerateImage(transcript, objectToActOnWithVoice, (transform, texture) =>
            {
                var progress = objectToActOnWithVoice.parent?.GetComponentInChildren<Progress>();
                if(progress != null)
                    progress.StopProgress();

                transform.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            });

            var frame = objectToActOnWithVoice.parent.GetComponentInChildren<Frame>();
            if(frame != null)
                frame.FramePrompt.text = transcript;
        });

        appVoiceExperience.events.onPartialTranscription.AddListener((transcript) =>
        {
            partialTranscriptText.text = transcript;
            var frame = objectToActOnWithVoice.parent.GetComponentInChildren<Frame>();
            if(frame != null)
                frame.FramePrompt.text = transcript;
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

    public void ActivateVoice(Transform selectedTransform)
    {
        objectToActOnWithVoice = selectedTransform;
        Logger.Instance.LogInfo("Update is about to activate voice");
        appVoiceExperience.Activate();
    }
}


