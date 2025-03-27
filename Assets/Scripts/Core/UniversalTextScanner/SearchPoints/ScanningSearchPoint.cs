using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UniversalText.Core
{
    public class ScanningSearchPoint : ISearchPoint
    {
        public string Description { get => "The user is scanning"; }

        private GameObject _scanner;
        private GameObject _scannerVisual;
        private float _scannerRadius;

        public ScanningSearchPoint(GameObject scanner)
        {   
            _scanner = scanner;
            _scannerVisual = scanner.transform.GetChild(0).gameObject;
            
            // Try to get the original Scanner component first
            var originalScanner = scanner.GetComponent<Scanner>();
            if (originalScanner != null)
            {
                _scannerRadius = originalScanner.ScanRadius * scanner.transform.localScale.x;
            }
            else
            {
                // Fall back to ScannerAlt
                var alternativeScanner = scanner.GetComponent<ScannerAlt>();
                if (alternativeScanner != null)
                {
                    _scannerRadius = alternativeScanner.ScanRadius * scanner.transform.localScale.x;
                }
                else
                {
                    Debug.LogError("No scanner component found on the provided GameObject");
                    _scannerRadius = 0f;
                }
            }
        }

        public List<UniversalTextTag> Search()
        { 
            List<UniversalTextTag> scanned = new List<UniversalTextTag>();

            Transform scannerTransform = _scanner.transform;
            MeshCollider scannerCollider = _scannerVisual.GetComponent<MeshCollider>();

            Collider[] hitColliders = Physics.OverlapSphere(scannerTransform.position, _scannerRadius);
        
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject == _scannerVisual || collider.gameObject.transform.root == _scanner.transform.parent.transform.root) continue;

                if (Physics.ComputePenetration(
                    collider,
                    collider.transform.position,
                    collider.transform.rotation,
                    scannerCollider,
                    scannerCollider.transform.position,
                    scannerCollider.transform.rotation,
                    out _,
                    out _))
                {
                    UniversalTextTag tag = collider.GetComponentInParent<UniversalTextTag>();
                    if (tag != null)
                    {
                        scanned.Add(tag);
                    }
                }
            }

            return scanned;
        }
    }
}

