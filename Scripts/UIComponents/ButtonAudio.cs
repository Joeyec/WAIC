using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    private AudioClip HoverClip;
    private AudioClip ClickClip;
    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = transform.GetComponent<AudioSource>();
        HoverClip = Resources.Load<AudioClip>("Audio/over");
        ClickClip = Resources.Load<AudioClip>("Audio/click");
	}
    
    public void HoverButton()
    {
        audioSource.clip = HoverClip;
        audioSource.Play();
    }

    public void ClickButton()
    {
        audioSource.clip = ClickClip;
        audioSource.Play();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
