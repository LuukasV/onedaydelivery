using UnityEngine;

// Rotates player body and camera.
public class PlayerCamera : MonoBehaviour
{
    // Displays values in Unity. Has no functionality in movement.
    public float MouseX;
    public float MouseY;
    public float Angle;

    // Sets sensitivity to player input from mouse.
    public float sensX;
    public float sensY;

    // Player's physical body transform data
    public Transform Body;
    // Camera transform data. Acts as players sight and head.
    public Transform Head;

    // Cursor is set to not visible and locked.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Rotates player based on mouse input.
    void Update()
    {
        MouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        Body.Rotate(Vector3.up, MouseX);

        MouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        Angle -= MouseY;
        Angle = Mathf.Clamp(Angle, -30, 45);
        Head.localRotation = Quaternion.Euler(Angle, 0, 0);
    }
}
