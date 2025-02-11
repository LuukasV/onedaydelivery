using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esimerkkiscripti liitetyn hahmon liikuttamiseen

public class MovementTest : MonoBehaviour
{
    // Julkiset attribuutit, n�kyv�t automaagisesti UI:ss�
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
        // Kierret��n y-akselin suhteen
	    transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Selvitet��n miss� on eteenp�in
	    var forward = transform.TransformDirection(Vector3.forward);
        // Liikkumisnopeus * sy�te vertikaali-inputista (n�pp�imist�lt� nuolet yl�s/alas 1/-1)
 	    var curSpeed = speed * Input.GetAxis("Vertical");

        // Liikutetaan
 	    controller.SimpleMove(forward * curSpeed);
    }
}
