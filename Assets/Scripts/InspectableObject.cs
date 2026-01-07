using UnityEngine;

public class InspectableObject : MonoBehaviour, IInteractable
{
    public float inspectDistance = 0.5f;

    private float rotationSpeed = 200;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isBeingInspected = false;
    private Transform cameraTransform;

    private PlayerController playerController;
    private SimpleSmoothMouseLook playerMouseLook;

    private GameObject canvas;

    public string interactText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.Find("UI Canvas");

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerMouseLook = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SimpleSmoothMouseLook>();

        // Cache the start position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Move the object to its start position
        // to try and compile the shader variant
        transform.position = startPosition;
        transform.rotation = startRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingInspected)
        {
            HandleRotation();
        }
    }

    public void Interact()
    {
        if (!isBeingInspected)
        {
            StartInspecting();
        }
        else
        {
            StopInspecting();
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    void StartInspecting()
    {
        AudioManager.Instance.PlayImmediate("Pick Up Item");

        isBeingInspected = true;

        canvas.SetActive(false);

        playerController.movementLocked = true;
        playerMouseLook.mouseLookLocked = true;

        transform.position = cameraTransform.position + cameraTransform.forward * inspectDistance;
        transform.rotation = cameraTransform.rotation;
    }

    void StopInspecting()
    {
        isBeingInspected = false;

        canvas.SetActive(true);

        playerController.movementLocked = false;
        playerMouseLook.mouseLookLocked = false;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    void HandleRotation()
    {
        // Get mouse movement on the X-axis
        float mouseX = Input.GetAxis("Mouse X");

        // Rotate the object around its Y-axis
        transform.Rotate(transform.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);
    }

}
