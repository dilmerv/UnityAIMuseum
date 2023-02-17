using OpenAI.Images;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    [SerializeField]
    private XRSimpleInteractable interactable;

    private Material materialInstance;

    private bool hasInteraction = false;

    private XRRayInteractor activeInteractor;

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

        if (interactable == null)
        {
            interactable = GetComponentInChildren<XRSimpleInteractable>();
        }
    }

    private void Awake()
    {
        framePrompt.text = string.Empty;
        frameProgress.text = string.Empty;
        materialInstance = imageRenderer.material;
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void Update()
    {
        if (hasInteraction)
        {
            if (activeInteractor.TryGetHitInfo(out var pointerPosition, out _, out _, out _))
            {
                Debug.Log(pointerPosition);
            }
        }
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

    private void OnHoverEntered(HoverEnterEventArgs hoverEnteredArgs)
    {
        if (hoverEnteredArgs.interactorObject is XRRayInteractor rayInteractor)
        {
            activeInteractor = rayInteractor;
            hasInteraction = true;
        }
    }

    private void OnHoverExit(HoverExitEventArgs hoverExitArgs)
    {
        if (hoverExitArgs.interactorObject is XRRayInteractor rayInteractor &&
            rayInteractor == activeInteractor)
        {
            hasInteraction = false;
            activeInteractor = null;
        }
    }
}
