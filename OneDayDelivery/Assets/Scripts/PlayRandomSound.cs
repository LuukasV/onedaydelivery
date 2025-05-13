using UnityEngine;

//Plays a random sound from a list of sound effects with the selected audiosource
public class PlayRandomSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] audioClips;

    void OnTriggerEnter()
    {
        //choose a random audioclip to play
        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];
        
        //play the audioclip with audiosource
        audioSource.PlayOneShot(randomClip);
    }
}