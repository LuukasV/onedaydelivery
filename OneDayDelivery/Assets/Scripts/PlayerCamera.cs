using UnityEngine;

// Rotates player body and camera.
public class PlayerCamera : MonoBehaviour
{
    // Displays values in Unity. Has no functionality in movement.
    public float MouseX;
    public float MouseY;
    public float Angle;

    // Sets mouse sensitivity to chosen Settings (stored in Player prefrences)
    //public float sensX;
    //public float sensY;
    private float mouseSens;

    [SerializeField] public int minValueAngle;
    [SerializeField] public int posValueAngle;

    // Player's physical body transform data
    public Transform Body;
    // Camera transform data. Acts as players sight and head.
    public Transform Head;

    // Cursor is set to not visible and locked.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSens = PlayerPrefs.GetFloat("CurrentMouseSensitivity", 200); //Mouse sensitivity defaults to 200 (stored in Player preferences)
    }

    // Rotates player based on mouse input.
    void Update()
    {
        MouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        Body.Rotate(Vector3.up, MouseX);

        MouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        Angle -= MouseY;
        Angle = Mathf.Clamp(Angle, minValueAngle, posValueAngle);
        Head.localRotation = Quaternion.Euler(Angle, 0, 0);
    }
}
