using DilmerGames.Core.Singletons;
using OpenAI;
using OpenAI.Images;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MuseumImageGenerator : Singleton<MuseumImageGenerator>
{
    [Header("Inputs")]
    [SerializeField]
    [TextArea(5, 20)]
    private string prompt;

    [SerializeField]
    private ImageSize imageSize = ImageSize.Small;

    public async void GenerateImage(string prompt, Transform transform, Action<Transform, Texture2D> callBack = null)
    {
        try
        {
            var api = new OpenAIClient();
            var results = await api.ImagesEndPoint.GenerateImageAsync(prompt, 1, imageSize);

            foreach (var result in results)
            {
                Debug.Log(result.Key);
                //transform.GetComponent<Renderer>().material.SetTexture("_MainTex", result.Value);
                callBack?.Invoke(transform, result.Value);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    public void OnFrameHovered(HoverEnterEventArgs args)
    {
        Logger.Instance.LogInfo(args.interactableObject.transform.name);
    }

    public void OnFrameSelected(SelectEnterEventArgs args)
    {
        Logger.Instance.LogInfo($"OnFrameSelected: {args.interactableObject.transform.name}");
        VoiceControllerWithPrompt.Instance.ActivateVoice(args.interactableObject.transform);
    }
}
