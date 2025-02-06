using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactDist = 1.5f;

    public TextMeshProUGUI interactText;

    [SerializeField]
    LayerMask mask = 0;

    private Transform cameraTransform;
    private IInteractable currentTarget;

    private string defaultInteractText = "[E]";


    void Awake()
    {
        cameraTransform = Camera.main.transform;
        interactText.text = null;
    }


    void Update()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * interactDist);

        if (Physics.Raycast(ray, out hitInfo, interactDist, mask))
        {

            // Hit and interactable
            if (hitInfo.transform.TryGetComponent(out IInteractable interactable))
            {
                currentTarget = interactable;
                interactText.text = defaultInteractText;
            }
            else // Hit something, but not  an interactable
            {
                interactText.text = null;
            }
        }
        else // Raycast didn't hit anything at all
        {
            currentTarget = null;
            interactText.text = null;
        }


        // Interact if there's an interactable target
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget != null)
            {
                currentTarget.Interact();
            }
        }
    }
}