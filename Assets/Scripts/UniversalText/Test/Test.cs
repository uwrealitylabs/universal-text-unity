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
    public Camera xrCamera;

    public float maxViewDistance = 10f;
    public float angleSpread = 15f;
    public int maxNumber = 3;
    
    void Start()
    {
        Debug.Log("TEST");
        UniversalTextScanner.Instance.AddSearchPoint(new NearbySearchPoint(xrCamera, maxViewDistance, maxNumber));
        UniversalTextScanner.Instance.AddSearchPoint(new GrabbingSearchPoint(leftHandGrabInteractor, rightHandGrabInteractor));
        UniversalTextScanner.Instance.AddSearchPoint(new PointingSearchPoint(rightHand, leftHand));
        //UniversalTextScanner.Instance.AddSearchPoint(new LookingSearchPoint(xrCamera, maxViewDistance));
        UniversalTextScanner.Instance.AddSearchPoint(new LookingInDirectionOfSearchPoint(xrCamera, maxViewDistance, angleSpread));
        UniversalTextScanner.Instance.Generate();
        StartCoroutine(PrintUTS());
    }

    private IEnumerator PrintUTS()
    {
        while (true)
        {
            Debug.Log("UTS:");
            Debug.Log(UniversalTextScanner.Instance.Generate());
            yield return new WaitForSeconds(1);
        }
    }
}
