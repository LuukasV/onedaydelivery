using UnityEngine;

public class MenuCameraSway : MonoBehaviour
{
    public float swayAmount = 0.1f;
    public float swaySpeed = 0.5f;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }

    void Update()
    {
        // Sway position slightly
        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float swayY = Mathf.Cos(Time.time * swaySpeed * 0.8f) * swayAmount * 0.5f;

        transform.localPosition = initialPosition + new Vector3(swayX, swayY, 0);

        // Sway rotation slightly
        float rotX = Mathf.Cos(Time.time * swaySpeed * 0.6f) * swayAmount * 5f;
        float rotY = Mathf.Sin(Time.time * swaySpeed * 0.4f) * swayAmount * 5f;

        transform.localEulerAngles = initialRotation + new Vector3(rotX, rotY, 0);
    }
}