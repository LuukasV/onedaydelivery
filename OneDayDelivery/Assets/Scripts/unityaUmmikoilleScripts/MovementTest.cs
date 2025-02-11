using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esimerkkiscripti liitetyn hahmon liikuttamiseen

public class MovementTest : MonoBehaviour
{
    // Julkiset attribuutit, näkyvät automaagisesti UI:ssä
	public float speed = 20.0f;
	public float rotateSpeed = 3.0f;

    // Yksityisen attribuutit
	private CharacterController controller;

    // Ajetaan, kun scriptin instanssi ladataan
    void Awake()
    {
	    controller = gameObject.GetComponent("CharacterController") as CharacterController;
    }

    // Ajetaan jokaisella framella
    void Update()
    {
        // Kierretään y-akselin suhteen
	    transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Selvitetään missä on eteenpäin
	    var forward = transform.TransformDirection(Vector3.forward);
        // Liikkumisnopeus * syöte vertikaali-inputista (näppäimistöltä nuolet ylös/alas 1/-1)
 	    var curSpeed = speed * Input.GetAxis("Vertical");

        // Liikutetaan
 	    controller.SimpleMove(forward * curSpeed);
    }
}
