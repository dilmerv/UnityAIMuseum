using OpenAI.Images;
using TMPro;
using UnityEngine;

public class Frame : MonoBehaviour
{
    private static readonly int mainTex = Shader.PropertyToID("_MainTex");

    [SerializeField]
    private ImageSize imageSize;

    public ImageSize ImageSize
    {
        get => imageSize;
        set => imageSize = value;
    }

    [SerializeField]
    private Renderer imageRenderer;

    [SerializeField]
    private TextMeshPro framePrompt;

    public TextMeshPro FrameProgress
    {
        get => frameProgress;
        set => frameProgress = value;
    }

    [SerializeField]
    private TextMeshPro frameProgress;

    public TextMeshPro FramePrompt
    {
        get => framePrompt;
        set => framePrompt = value;
    }

    private Material materialInstance;

    private void OnValidate()
    {
        if (framePrompt == null)
        {
            throw new MissingReferenceException(nameof(framePrompt));
        }

        if (frameProgress == null)
        {
            throw new MissingReferenceException(nameof(framePrompt));
        }

        if (imageRenderer == null)
        {
            imageRenderer = GetComponentInChildren<Renderer>();
        }
    }

    private void Awake()
    {
        framePrompt.text = string.Empty;
        frameProgress.text = string.Empty;
        materialInstance = imageRenderer.material;
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }

    public void UpdateTexture(Texture2D texture)
    {
        materialInstance.SetTexture(mainTex, texture);
    }
}
