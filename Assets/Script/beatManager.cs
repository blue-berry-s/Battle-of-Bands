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
        foreach (Intervals interval in _intervals) {
            // times elapsed in beats
            float sampleTime = (_audioSource.timeSamples / _audioSource.clip.frequency * interval.GetIntervalLength(_bpm));
            interval.CheckForNewInterval(sampleTime);
        }
    }
}

[System.Serializable]
public class Intervals {
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int lastInterval;

    // Leanth of current beat
    public float GetIntervalLength(float bpm) {
        return 60f / (bpm * _steps);
    }
    // if we're at a new beat or not
    public void CheckForNewInterval(float interval) {
        if (Mathf.FloorToInt(interval) != lastInterval) {
            lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
    
}
