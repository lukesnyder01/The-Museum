using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public GameObject target;
    private IActivatable targetActivatable;

    private Vector3 depressTargetPos;
    private float pressDistance = 0.02f;
    private float returnTime = 0.1f;
    private Vector3 startPosition;

    public string interactText;

    void Start()
    {
        if (target != null && target.TryGetComponent<IActivatable>(out IActivatable activatable))
        {
            targetActivatable = activatable;
        }

        startPosition = transform.position;
        depressTargetPos = startPosition + -transform.up * pressDistance;
    }


    public void Interact()
    {
        StartCoroutine(DepressButton());
        AudioManager.Instance.PlayImmediate("Click Switch");

        if (targetActivatable != null)
        {
            targetActivatable.Activate();
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    IEnumerator DepressButton()
    {
        transform.position = depressTargetPos;
        yield return new WaitForSeconds(returnTime);
        transform.position = startPosition;
        yield return null;
    }

    

}
