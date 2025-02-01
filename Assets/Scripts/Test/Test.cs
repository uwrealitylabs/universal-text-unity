using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;

public class Test : MonoBehaviour
{
    public HandGrabInteractor leftHandGrabInteractor;
    public HandGrabInteractor rightHandGrabInteractor;
    public Hand leftHand;
    public Hand rightHand;
    

    void Start()
    {
        UniversalTextScanner.Instance.AddSearchPoint(new GrabbingSearchPoint(leftHandGrabInteractor, rightHandGrabInteractor));
        Debug.Log(rightHand);
        Debug.Log(leftHand);
        UniversalTextScanner.Instance.AddSearchPoint(new PointingSearchPoint(rightHand, leftHand));
        UniversalTextScanner.Instance.Generate();
        StartCoroutine(PrintUTS());
    }

    private IEnumerator PrintUTS()
    {
        while (true)
        {
            Debug.Log(UniversalTextScanner.Instance.Generate());
            yield return new WaitForSeconds(1);
        }
    }
}
