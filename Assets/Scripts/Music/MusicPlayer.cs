using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public enum Notes
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G
    }

    [SerializeField]
    private List<AudioClip> notes;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        audioSource.pitch = 2.0f;
    }

    public void Play(Notes note)
    {
        audioSource.PlayOneShot(notes[(int)note]);
    }
}
