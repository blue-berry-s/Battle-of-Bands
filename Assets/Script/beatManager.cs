using UnityEngine;
using UnityEngine.Events;
public class beatManager : MonoBehaviour
{

    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public Intervals[] _intervals;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedSeconds = (float)_audioSource.timeSamples / _audioSource.clip.frequency;
        foreach (Intervals interval in _intervals) {
            float sampleTime = elapsedSeconds / interval.GetIntervalLength(_bpm);

            interval.CheckForNewInterval(sampleTime);
        }
    }
}

[System.Serializable]
public class Intervals {
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    [SerializeField] private Metronome _metronome;
    public int lastInterval;
    public int currentInterval;
    public float _interval;

    // Leanth of current beat
    public float GetIntervalLength(float bpm) {
        return 60f / (bpm * _steps);
    }
    // if we're at a new beat or not
    public void CheckForNewInterval(float interval) {
        _interval = interval;
        //Debug.Log(interval);
        currentInterval = Mathf.FloorToInt(interval);

        if (currentInterval < lastInterval)
        {
            lastInterval = currentInterval;
            return;
        }

        else if (interval % 1 >= 0.85 && interval % 1 <= 0.9999999f && currentInterval == lastInterval)
        {
            _metronome.attackPeriod = true;
        }       
        if (currentInterval != lastInterval)
        {
            lastInterval = currentInterval;
            _trigger.Invoke();
        }
        
    }
    
}
