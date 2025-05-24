using UnityEngine;

/// <summary>
/// Main programmer: Jussi Kolehmainen
/// Other programmer: Luukas Vuolle
/// Majority of the programming was done by the main programmer. Other programmer added a line of code 
/// for linking mouse sensitivity to UI/Player preferences
/// 
/// Rotates player body and camera.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    // Displays values in Unity. Has no functionality in movement.
    [Header("Angle and inputs")]
    public float MouseX;
    public float MouseY;
    public float Angle;

    // Sets mouse sensitivity to chosen Settings (stored in Player prefrences)
    private float mouseSens;

    [Header("Max and min angle where player can look")]
    [SerializeField] private int minValueAngle;
    [SerializeField] private int posValueAngle;

    [Header("Transform variables for movement and camera functions")]
    // Player's physical body transform data
    public Transform Body;
    // Camera transform data. Acts as players sight and head.
    public Transform Head;

    /// <summary>
    /// Cursor is set to not visible and locked.
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSens = PlayerPrefs.GetFloat("CurrentMouseSensitivity", 100); //Mouse sensitivity defaults to 200 (stored in Player preferences)
    }

    /// <summary>
    /// Rotates player based on mouse input.
    /// </summary>
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
