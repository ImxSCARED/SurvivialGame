using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public LayerMask interactablelayers;
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;


    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
            interact();
    }

    private void interact()
   {     RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactablelayers))
      {
            Pickup pickup = hit.transform.GetComponent<Pickup>();

            if (pickup != null)
            {
               GetComponentInParent<WindowHandler>().inventory.AddItem(pickup);
            }
       }
    }
}
