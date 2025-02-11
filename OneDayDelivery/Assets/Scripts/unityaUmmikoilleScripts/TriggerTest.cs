using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esimerkkiscripti, miten laukaista tapahtumia

public class TriggerTest : MonoBehaviour
{
    // Julkiset attribuutit, n‰kyv‰t editorissa
    public GameObject source; // L‰hde, aseta pelaajaksi
    public GameObject target; // Kohde, mik‰ aktivoidaan tai deaktivoidaan
    public AudioSource audioSource; // ƒ‰ni, joka soitetaan kun trigger aktivoituu

    // Enumeraattori helpottamaan el‰m‰‰ ja tekem‰‰n UI siistimmi‰ksi
    public enum Mode
    {
        Activate = 0, // Aktivoidaan kohde GameObject
        Deactivate = 1, // Deaktivoidaan kohde GameObject
    }

    public Mode action = Mode.Activate;

    // Kutsutaan kun triggertapahtuma alkaa
    void OnTriggerEnter(Collider other)
    {
        ActivateTrigger(other);
    }

    // Oma k‰sittely, modesta riippuen aktivoidaan tai deaktivoidaan
    void ActivateTrigger(Collider activator)
    {

        //soitetaan ‰‰ni, jos m‰‰ritelty
        if (audioSource)
            audioSource.Play(0);

        switch (action)
        {
            case Mode.Activate:
                if (activator.transform == source.transform)
                    target.SetActive(true);
                break;
            case Mode.Deactivate:
                if (activator.transform == source.transform)
                    target.SetActive(false);
                break;
        }
    }
}