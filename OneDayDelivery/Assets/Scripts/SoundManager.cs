using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//TODO: find out if this is actually needed
public enum SoundType
{
    //when you add a sound effect, add it to the list here
}


[RequireComponent(typeof(AudioSource))]
//Sound effect management in unity scene. Allows playing global sound effects and adding them
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList; //sound effects can be added to the list in unity inspector
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //play sound from the soundList
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound]); 
    }
}
