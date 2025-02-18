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
        // Rotate the paintings towards its current rotation on start
        // to prevent the weird material/shader issue that shows
        // the material as light blue for a few frames the first time
        // you move a painting.

        // Theoretically, it's something to do with the shader variant compiling
        // at runtime, and I think this forces it to compile right at the start
        // while we can't even see it.

        targetRotation = transform.rotation;
        Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
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
