using DilmerGames.Core.Singletons;
using OpenAI;
using OpenAI.Images;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAITests : Singleton<OpenAITests>
{
    [Header("UI")]
    [SerializeField]
    private RawImage aiImage;

    [SerializeField]
    private Button generateImage;

    [SerializeField]
    private TextMeshProUGUI promptText;

    [Header("Inputs")]

    [SerializeField]
    [TextArea(5, 20)]
    private string prompt;

    [SerializeField]
    private ImageSize imageSize = ImageSize.Small;

    private void Awake()
    {
        generateImage.onClick.AddListener(() =>
        {
            promptText.text = prompt;
            GenerateImage(prompt);
        });
    }

    public async void GenerateImage(string overridePrompt)
    {
        try
        {
            generateImage.interactable = false;
            promptText.text = string.IsNullOrEmpty(overridePrompt) ? prompt : overridePrompt;
            var api = new OpenAIClient();
            var results = await api.ImagesEndPoint.GenerateImageAsync(promptText.text, 1, imageSize);

            foreach (var result in results)
            {
                Debug.Log(result.Key);
                aiImage.texture = result.Value;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

        generateImage.interactable = true;
    }
}
