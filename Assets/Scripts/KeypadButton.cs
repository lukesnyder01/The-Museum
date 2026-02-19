using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour, IInteractable
{
    public KeypadLock keypadLock;

    public int digit;

    private Vector3 depressTargetPos;
    private float pressDistance = 0.01f;
    private float returnTime = 0.1f;
    private Vector3 startPosition;

    public string interactText;

    void Start()
    {
        startPosition = transform.position;
        depressTargetPos = startPosition + -transform.up * pressDistance;
        interactText = digit.ToString();
    }


    public void Interact()
    {
        StartCoroutine(DepressButton());
        AudioManager.Instance.PlayImmediate("Click Switch");

        keypadLock.EnterDigit(digit);
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
