using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main programmer: Luukas Vuolle
/// 
/// Inner mechanics of CompassIcon Class
/// </summary>
public class CompassIcon : MonoBehaviour
{
    public Sprite icon;
    public Image image;
    private int id = -1;

    public Vector2 position
    {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }

    //Sets the Icons ID value, for detection purposes
    public void setID(int id)
    {
        this.id = id;
    }

    //Gives the icon's (and the postboxes id) for identification purposes
    public int getID()
    {
        return id;
    }
}
