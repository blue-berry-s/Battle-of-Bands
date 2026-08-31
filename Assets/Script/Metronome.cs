using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{

    private int currentbeat = 0;
    private int maxBeats;
    [SerializeField] private Color empty;
    [SerializeField] private Color full;
    [SerializeField] private Image[] beatImages;

    [SerializeField] private float pluseSize = 1.15f;
    [SerializeField] private float returnSpeed = 5f;

    private RectTransform rectTransform;
    private Vector3 startSize;

    private void Start()
    {
        maxBeats = beatImages.Length - 1;
        rectTransform = GetComponent<RectTransform>();
        startSize = transform.localScale;
        beat();
        
    }

    private void Update()
    {
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void beat() {
        clearPrevBeat();
        beatImages[currentbeat].color = full;
        pulse();


        if (currentbeat + 1 > maxBeats)
        {
            currentbeat = 0;
        }
        else {
            currentbeat++;
        }
    }

    public void pulse() {
        if (rectTransform != null)
        {
            // Instantly snap to the expanded pulse scale
            rectTransform.localScale = startSize * pluseSize;
        }
    }

    private void clearAllBeats() {
        foreach (Image i in beatImages) {
            i.color = empty;
        }
    }

    private void clearPrevBeat()
    {
        if (currentbeat == 0) {
            beatImages[beatImages.Length - 1].color = empty;
        }
        else
        {
            beatImages[currentbeat - 1].color = empty;
        }
    }
}
