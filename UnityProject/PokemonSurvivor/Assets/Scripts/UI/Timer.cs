using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI timerText_L;

    public float lastReset;
    public float timerTime
    {
        get
        {
            return Time.time - lastReset;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lastReset = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        string seconds = ((int)(timerTime % 60f)).ToString();
        string minutes = ((int)(timerTime / 60f)).ToString();
        if (seconds.Length == 1)
        {
            seconds = "0" + seconds;
        }
        if (minutes.Length == 1)
        {
            minutes = "0" + minutes;
        }
        timerText_L.text = minutes + ":" + seconds;
    }
}
