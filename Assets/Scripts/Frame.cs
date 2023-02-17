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

    private bool hasInteraction;

    private XRRayInteractor activeInteractor;

    public Texture2D Image { get; private set; }

    // TODO to be created after done drawing
    public Texture2D Mask { get; private set; }

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
        materialInstance = imageRenderer.material;
    }

    private void OnEnable()
    {
        framePrompt.text = string.Empty;
        frameProgress.text = string.Empty;
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void Update()
    {
        if (hasInteraction &&
            // TODO ideally we should be checking if the trigger is pulled here not grab
            // but I wasn't sure exactly how to filter for that.
            activeInteractor.isSelectActive &&
            activeInteractor.TryGetHitInfo(out var pointerPosition, out _, out _, out _))
        {
            // TODO send pointer position to the shader to draw new pixels in alpha channel
            Debug.Log(pointerPosition);
        }
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }

    public void UpdateImage(Texture2D texture)
    {
        Image = texture;
        materialInstance.SetTexture(mainTex, Image);
    }

    public void ClearImage()
    {
        Image = null;
        materialInstance.SetTexture(mainTex, null);
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
