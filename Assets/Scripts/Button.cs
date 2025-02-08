using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public GameObject target;
    private IActivatable targetActivatable;

    private Vector3 depressTargetPos;
    private float pressDistance = 0.05f;
    private float returnTime = 0.1f;
    private Vector3 startPosition;

    private AudioSource audioSource;

    void Start()
    {
        if (target != null && target.TryGetComponent<IActivatable>(out IActivatable activatable))
        {
            targetActivatable = activatable;
        }

        startPosition = transform.position;
        depressTargetPos = startPosition + -transform.up * pressDistance;
        audioSource = GetComponent<AudioSource>();

    }


    public void Interact()
    {
        Debug.Log("Pressed Button");
        StartCoroutine(DepressButton());
        audioSource.Play();

        if (targetActivatable != null)
        {
            targetActivatable.Activate();
        }


    }


    IEnumerator DepressButton()
    {
        transform.position = depressTargetPos;
        yield return new WaitForSeconds(returnTime);
        transform.position = startPosition;
        yield return null;
    }

    

}
