using System.Collections;
using UnityEngine;

public class RotatablePainting: MonoBehaviour, IActivatable
{
    private float rotationSpeed = 30f;

    private Vector3 startingUpVector;

    private bool isRotating = false;

    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    public void Activate()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateSmoothly());
        }
    }

    private IEnumerator RotateSmoothly()
    {
        isRotating = true;

        targetRotation *= Quaternion.Euler(0, 0, 90);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation; // Snap to targetRotation

        isRotating = false;
    }

}
