using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script to make the UI's compass work
public class CompassManager : MonoBehaviour
{
    public GameObject iconPrefab;
    List<CompassIcon> compassIcons = new List<CompassIcon>();

    public RawImage compassImage;
    public Transform player;

    float compassUnit;

    //TODO: Add CompassFlags from UIManager?
    public CompassIcon compassIcon1;
    public CompassIcon compassIcon2;
    public CompassIcon compassIcon3;
    public CompassIcon compassIcon4;

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        //TODO: Add CompassFlags from UIManager?
        AddCompassIcon(compassIcon1);
        AddCompassIcon(compassIcon2);
        AddCompassIcon(compassIcon3);
        AddCompassIcon(compassIcon4);
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
        }
    }

    //Adds a new icon to the Compass
    public void AddCompassIcon(CompassIcon icon)
    {
        GameObject newIcon = Instantiate(iconPrefab, compassImage.transform);
        icon.image = newIcon.GetComponent<Image>();
        icon.image.sprite = icon.icon;

        compassIcons.Add(icon);
    }

    //Position Compass Icon correctly based on player and Compass Flag position
    Vector2 GetPosOnCompass(CompassIcon icon)
    {

        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(icon.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);

    }
}
