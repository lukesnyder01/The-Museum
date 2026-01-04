using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IActivatable
{
    public Vector3 closedPosition;
    public Quaternion closedRotation;

    public Vector3 openPosition;
    public Quaternion openRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public float rotationSpeed = 80f;
    public float moveSpeed = 0.5f;

    private bool doorIsClosed = true;
    private bool doorIsMoving = false;

    public bool doorCanClose = true;


    [ContextMenu("Set Closed Position And Rotation")]
    private void SetClosedState()
    {
        closedPosition = transform.position;
        closedRotation = transform.rotation;
    }


    [ContextMenu("Set Open Position And Rotation")]
    private void SetOpenState()
    {
        openPosition = transform.position;
        openRotation = transform.rotation;
    }


    public void Activate()
    {
        if (doorIsMoving == false)
        {
            if (doorIsClosed)
            {
                targetPosition = openPosition;
                targetRotation = openRotation;

                doorIsClosed = false;
            }
            else if (doorCanClose)
            {
                targetPosition = closedPosition;
                targetRotation = closedRotation;

                doorIsClosed = true;
            }

            StartCoroutine(RotateAndMoveDoor());
        }
    }



    private IEnumerator RotateAndMoveDoor()
    {
        doorIsMoving = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f || Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            

            yield return null;
        }

        // Ensure final position and rotation are set exactly to avoid precision errors
        transform.rotation = targetRotation;
        transform.position = targetPosition;

        doorIsMoving = false;
    }



}
