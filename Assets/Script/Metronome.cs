using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Metronome : MonoBehaviour
{

    private int currentbeat = 0;
    private int maxBeats;
    [SerializeField] public Color empty;
    [SerializeField] private Color full;
    [SerializeField] private Image[] beatImages;

    [SerializeField] private float BeatpluseSize = 1.15f;
    [SerializeField] private float returnSpeed = 10f;
    //80ms of linencey
    private float linentTime = 0.15f;

    private RectTransform rectTransform;
    private Vector3 startSize;

    public bool attackPeriod { get; set; } = false;
    public int penaltyDamage { get; private set; } = 1;
    

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

        StartCoroutine(closeAttackperiod());
    }

    public void pulse() {
        rectTransform.localScale = startSize * BeatpluseSize;
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

    IEnumerator closeAttackperiod() {
        yield return new WaitForSeconds(linentTime);
        attackPeriod = false;
        //Debug.Log("CLOSED");
    }
}
