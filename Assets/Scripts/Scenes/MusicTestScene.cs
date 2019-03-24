using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTestScene : MonoBehaviour
{


    [SerializeField]
    private AudioClip _a, _b, _c, _d, _e, _f, _g;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 2.0f;
    }

    public void PlayA()
    {
        audioSource.PlayOneShot(_a);
    }
    public void PlayB()
    {
        audioSource.PlayOneShot(_b);
    }
    public void PlayC()
    {
        audioSource.PlayOneShot(_c);
    }
    public void PlayD()
    {
        audioSource.PlayOneShot(_d);
    }
    public void PlayE()
    {
        audioSource.PlayOneShot(_e);
    }
    public void PlayF()
    {
        audioSource.PlayOneShot(_f);
    }
    public void PlayG()
    {
        audioSource.PlayOneShot(_g);
    }
}
