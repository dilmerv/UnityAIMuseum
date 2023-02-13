using DilmerGames.Core.Singletons;
using System.Collections;
using TMPro;
using UnityEngine;

public class Progress : Singleton<Progress> 
{
    [SerializeField]
    private TextMeshPro progressText = null;

    [SerializeField]
    private float frequency = 1.0f;

    [SerializeField]
    private int maxDots = 5;

    public string Status { get; set; }

    private int dotCount = 0;

    private bool done = false;

    private void Awake()
    {
        progressText.text = string.Empty;
    }

    public void StartProgress(string status = "In Progress...")
    {
        this.Status = status;
        progressText.gameObject.SetActive(true);
        StartCoroutine(ProcessProgress());
    }
    public void StopProgress()
    {
        done = true;
    }

    IEnumerator ProcessProgress()
    {
        while (true)
        {
            if(dotCount >= maxDots) dotCount = 0;

            progressText.text = $"{Status}{Dots(dotCount)}";
            dotCount++;

            if (done)
            {
                done = false;
                progressText.gameObject.SetActive(false);
                break;
            };

            yield return new WaitForSeconds(frequency);
        }
    }

    private string Dots(int count)
    {
        string dots = string.Empty;
        for (int i = 0; i < count; i++) dots += ".";
        return dots;
    }
}
