using UnityEngine;
using System.Collections;

/// <summary>
/// Programmer: Milo Hankama
/// Plays a random sound from a list of sound effects with the selected audiosource between random time intervals
/// </summary>
public class PlayRandomSoundTimed : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    [Header("Timing Settings")]
    public float minDelay = 5f; //Minimum time between sounds
    public float maxDelay = 10f; //Maximum time between sound

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); // Try to auto-assign
        }

        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);  //pick a wait time between minDelay and maxDelay
            yield return new WaitForSeconds(waitTime);

            if (audioClips.Length > 0)
            { 
                //choose a random audioclip to play
                AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];

                //play the audioclip with audiosource
                audioSource.PlayOneShot(randomClip);
            }
        }
    }
}
