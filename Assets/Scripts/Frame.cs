using TMPro;
using UnityEngine;

public class Frame : MonoBehaviour
{
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

    private void Awake()
    {
        framePrompt.text = string.Empty;
        frameProgress.text = string.Empty;
    }
}
