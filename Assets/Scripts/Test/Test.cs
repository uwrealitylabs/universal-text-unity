using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

public class Test : MonoBehaviour
{
    public HandGrabInteractor leftHandGrabInteractor;
    public HandGrabInteractor rightHandGrabInteractor;
    public Camera xrCamera;
    public float maxViewDistance = 10f;
    

    void Start()
    {
        UniversalTextScanner.Instance.AddSearchPoint(new GrabbingSearchPoint(leftHandGrabInteractor, rightHandGrabInteractor));
        UniversalTextScanner.Instance.AddSearchPoint(new LookingSearchPoint(xrCamera, maxViewDistance));
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
