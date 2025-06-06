using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main programmer: Luukas Vuolle
/// Code based on b3agz's RPG Guest Compass: https://www.youtube.com/watch?v=MRAVwaGrmrk
/// 
/// Script to make the UI's compass work
/// </summary>
public class CompassManager : MonoBehaviour
{
    public GameObject iconPrefab;
    List<CompassIcon> compassIcons = new List<CompassIcon>();

    public RawImage compassImage;
    public Transform player;

    public float maxMarkerDistance = 200f;

    float compassUnit;

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        //Adds all CompassFlag-tags to the compass
        GameObject[] compassTagObjects;
        compassTagObjects = GameObject.FindGameObjectsWithTag("CompassFlag");
        foreach (GameObject flag in compassTagObjects)
        {
            CompassIcon icon = flag.GetComponent<CompassIcon>();
            AddCompassIcon(icon);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        //We set the image to "wrap" around itself
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        //We update all icons position on the Compass
        foreach(CompassIcon icon in compassIcons)
        {
            icon.image.rectTransform.anchoredPosition = GetPosOnCompass(icon);

            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), icon.position);
            float scale = 0f;

            //We hide all icons that are too far away (scale stays at 0 if we are too far)
            if(distance < maxMarkerDistance) scale = 1f - (distance / maxMarkerDistance);

            icon.image.rectTransform.localScale = Vector3.one * scale;
        }
    }

    /// <summary>
    /// Adds a new icon to the Compass
    /// </summary>
    public void AddCompassIcon(CompassIcon icon)
    {
        GameObject newIcon = Instantiate(iconPrefab, compassImage.transform);
        icon.image = newIcon.GetComponent<Image>();
        icon.image.sprite = icon.icon;

        compassIcons.Add(icon);
    }

    /// <summary>
    /// Position of Compass Icon based on player and Compass Flag position
    /// </summary>
    /// <param name="icon"> Compass Icon which position is used</param>
    Vector2 GetPosOnCompass(CompassIcon icon)
    {

        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(icon.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);

    }

    /// <summary>
    /// Disablees the icon in the compass with the given id
    /// </summary>
    /// <param name="idOfIcon"> ID of Icon which is to disabled </param>
    public void disableSpecificIcon(int idOfIcon)
    {
        for (int i = 0; i < compassIcons.Count; i++)
        {
            CompassIcon icon = compassIcons[i];
            if (icon.getID()  == idOfIcon)
            {
                Destroy(icon.image.gameObject);
                Destroy(icon.gameObject);
                compassIcons.Remove(icon);
                i--;
            }
        }
    }
}
