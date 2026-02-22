using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeypadLock: MonoBehaviour
{
    public int passcode;

    private int length;

    private int[] enteredDigits;
    private int[] correctDigits;

    public GameObject unlockTarget;


    private void Start()
    {
        string passcodeString = passcode.ToString();
        length = passcodeString.Length;
        enteredDigits = new int[length];
        correctDigits = new int[length];

        for (int i = 0; i < length; i++)
        {
            correctDigits[i] = int.Parse(passcodeString[i].ToString());
        }
    }


    public void EnterDigit(int digit)
    {
        for (int i = 0; i < length - 1; i++)
        {
            enteredDigits[i] = enteredDigits[i + 1];
        }

        enteredDigits[length - 1] = digit;

        CheckPasscode();
    }


    public void CheckPasscode()
    {
        bool passcodeCorrect = true;

        for (int i = 0; i < length; i++)
        {
            if (enteredDigits[i] != correctDigits[i])
                passcodeCorrect = false;
        }

        Debug.Log(string.Join("", enteredDigits));

        if (passcodeCorrect)
        {
            UnlockDoor();
        }
    }


    private void UnlockDoor()
    {
        Debug.Log("Passcode correct, door unlocking!");
        if (unlockTarget.TryGetComponent<IUnlockable>(out IUnlockable unlockable))
        {
            unlockable.Unlock();
        }
        else
        {
            Debug.LogWarning("No valid unlock IUnlockable found");
        }
    }


}
