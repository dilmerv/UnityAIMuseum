using DilmerGames.Core.Singletons;
using OpenAI;
using OpenAI.Images;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ImageGenerator : Singleton<ImageGenerator>
{
    private OpenAIClient openAI;
    private OpenAIClient OpenAI => openAI ??= new OpenAIClient();

    public async void GenerateImage(string prompt, Transform targetTransform, ImageSize imageSize = ImageSize.Small, Action<Transform, Texture2D> callBack = null)
    {
        try
        {
            var results = await OpenAI.ImagesEndPoint.GenerateImageAsync(prompt, size: imageSize);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                callBack?.Invoke(targetTransform, texture);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public async void GenerateImageEdit(string prompt, Transform targetTransform, Texture2D baseImage, Texture2D imageMask, ImageSize imageSize = ImageSize.Small, Action<Transform, Texture2D> callBack = null)
    {
        try
        {
            var results = await OpenAI.ImagesEndPoint.CreateImageEditAsync(baseImage, imageMask, prompt, size: imageSize);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                callBack?.Invoke(targetTransform, texture);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void OnFrameHovered(HoverEnterEventArgs args)
    {
        Logger.Instance.LogInfo(args.interactableObject.transform.name);
    }

    public void OnFrameHoverExit(HoverExitEventArgs args)
    {
        Logger.Instance.LogInfo(args.interactableObject.transform.name);
    }

    public void OnFrameSelected(SelectEnterEventArgs args)
    {
        Logger.Instance.LogInfo($"OnFrameSelected: {args.interactableObject.transform.name}");
        VoiceControllerWithPrompt.Instance.ActivateVoice(args.interactableObject.transform);
    }
}
