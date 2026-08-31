using UnityEngine;
using UnityEngine.UI;
public class Beat : MonoBehaviour
{
    private Image currentImage;
    private Color startColor;
    private float returnSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentImage = gameObject.GetComponent<Image>();
        startColor = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>().empty;


    }

    // Update is called once per frame
    void Update()
    {
        currentImage.color = Color.Lerp(currentImage.color, startColor, Time.deltaTime * returnSpeed);
    }
}
