using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sorse : MonoBehaviour
{
    public AudioClip buttonHoverSound; // เสียงเมื่อเลื่อนเมาส์เหนือ Button
    public AudioClip buttonClickSound; // เสียงเมื่อคลิกปุ่ม
    public AudioSource audioSource;

    public void PlayButtonHoverSound()
    {
        audioSource.PlayOneShot(buttonHoverSound);
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
