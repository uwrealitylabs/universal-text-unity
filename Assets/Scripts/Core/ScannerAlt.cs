using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

public class ScannerAlt : MonoBehaviour
{   
    [SerializeField] private float scanRadius = 10f;
    [SerializeField] private float scanEdge = 0.2f;
    
    public float ScanRadius => scanRadius;
    public float ScanEdge => scanEdge;

    private GameObject scannerVisual;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Awake()
    {
        InitializeScannerMesh();
    }

    private void InitializeScannerMesh()
    {
        scannerVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        scannerVisual.transform.SetParent(transform, false);
        
        scannerVisual.transform.localPosition = new Vector3(0, 0, scanRadius);
        
        Vector3 parentScale = transform.lossyScale;
        scannerVisual.transform.localScale = new Vector3(
            scanEdge / parentScale.x,
            scanEdge / parentScale.y,
            scanEdge / parentScale.z
        );
        
        meshRenderer = scannerVisual.GetComponent<MeshRenderer>();
        
        Material scannerMaterial = new Material(Shader.Find("Standard"));
        scannerMaterial.color = new Color(0.5f, 0.8f, 1f, 0.15f); 
        
        scannerMaterial.SetFloat("_Mode", 3); 
        scannerMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        scannerMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        scannerMaterial.SetInt("_ZWrite", 0);
        scannerMaterial.DisableKeyword("_ALPHATEST_ON");
        scannerMaterial.EnableKeyword("_ALPHABLEND_ON");
        scannerMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        scannerMaterial.renderQueue = 3000;

        meshRenderer.material = scannerMaterial;

        Destroy(scannerVisual.GetComponent<BoxCollider>());
        meshCollider = scannerVisual.AddComponent<MeshCollider>();
        meshCollider.convex = false;
        meshCollider.isTrigger = true;
    }

    public void ToggleScanner()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}