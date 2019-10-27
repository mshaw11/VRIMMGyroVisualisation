using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LinearNotePlayer : MonoBehaviour
{
    [SerializeField]
    MusicPlayer musicPlayer;

    DateTime lastNotePlayedTime;

    [SerializeField]
    Slider thresholdSlider;

    [SerializeField]
    Slider minGapSlider;

    [SerializeField]
    Text magnitudeThresholdLabel, notesGapLabel, magnitudeLabel;

    // Start is called before the first frame update
    void Start()
    {
        lastNotePlayedTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        var magnitudeTriggerThreshold = ScaleBasedOnMinMax(thresholdSlider.value, 0, 10);
        var noteTimeThreshold = ScaleBasedOnMinMax(minGapSlider.value, 0, 2000);
        var magnitude = Input.acceleration.magnitude;

        if (magnitude > magnitudeTriggerThreshold) 
        {
            var currentTime = DateTime.Now;
            TimeSpan timeDiff = currentTime - lastNotePlayedTime;
            if (timeDiff.TotalMilliseconds > noteTimeThreshold)
            {
                musicPlayer.Play(MusicPlayer.Notes.A);
            }
        }

        magnitudeThresholdLabel.text = magnitudeTriggerThreshold.ToString("F0") + "g";
        notesGapLabel.text = noteTimeThreshold.ToString("F0") + "ms";
        magnitudeLabel.text = magnitude.ToString("F0") + "g";
    }

    float ScaleBasedOnMinMax(float zeroToOneValue, float min, float max)
    {
        var diff = max - min;
        var scale = diff;
        return min + (scale * zeroToOneValue);
    }
}
