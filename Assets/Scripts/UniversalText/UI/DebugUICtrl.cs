using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniversalText;

public class DebugUICtrl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _currentTime;
    [SerializeField] TextMeshProUGUI _rawRTR;
    [SerializeField] TextMeshProUGUI _enhancedRTR;

    // Start is called before the first frame update
    void Start()
    {
        if (_rawRTR != null)
        {
            _rawRTR.text = "";
            UniversalTextManager.Instance.RTRGenerated += (rtr, timestamp) =>
            {
                _rawRTR.text = $"Raw RTR [Requested @ {timestamp}]: {rtr}";
            };
        }
        if (_enhancedRTR != null)
        {
            _enhancedRTR.text = "";
            UniversalTextManager.Instance.EnhancedRTRGenerated += (enhanced, timestamp) =>
            {
                _enhancedRTR.text = $"Enhanced RTR [Requested @ {timestamp}]: {enhanced}";
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime.text = System.DateTime.Now.ToString();
    }
}
