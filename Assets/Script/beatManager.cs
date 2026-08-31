using UnityEngine;
using UnityEngine.Events;
public class beatManager : MonoBehaviour
{

    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;
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
    private int lastInterval;

    // Leanth of current beat
    public float GetIntervalLength(float bpm) {
        return 60f / (bpm * _steps);
    }
    // if we're at a new beat or not
    public void CheckForNewInterval(float interval) {
        //Debug.Log(interval);
        int currentInterval = Mathf.FloorToInt(interval);
        if (interval%1 >= 0.9 && interval% 1 < 0.94 && currentInterval == lastInterval) {
            _metronome.attackPeriod = true;
        }
        else if (currentInterval < lastInterval)
        {
            lastInterval = currentInterval;
            return;
        }

        if (currentInterval != lastInterval)
        {
            lastInterval = currentInterval;
            _trigger.Invoke();
        }
        
    }
    
}
