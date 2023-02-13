using TMPro;
using UnityEngine;

public class Frame : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro framePrompt;

    [SerializeField]
    private TextMeshPro frameProgress;

    public TextMeshPro FrameProgress
    {
        get { return frameProgress; }
        set { frameProgress = value; }
    }

    public TextMeshPro FramePrompt
    {
        get { return framePrompt; }
        set { framePrompt = value; }
    }

    private void Awake()
    {
        framePrompt.text = string.Empty;
        frameProgress.text = string.Empty;
    }
}
