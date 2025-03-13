using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

public class Scanner : MonoBehaviour
{   
    [SerializeField] private float scanRadius = 5f;
    [SerializeField] private float scanAngle = 20f;
    
    public float ScanRadius => scanRadius;
    public float ScanAngle => scanAngle;

    private GameObject scannerVisual;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh scannerMesh;
    private MeshCollider meshCollider;

    private void Awake()
    {
        InitializeScannerMesh();
    }

    private void InitializeScannerMesh()
    {
        scannerVisual = new GameObject("ScannerVisual");
        scannerVisual.transform.SetParent(transform, false); 
        
        meshFilter = scannerVisual.AddComponent<MeshFilter>();
        meshRenderer = scannerVisual.AddComponent<MeshRenderer>();

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

        meshCollider = scannerVisual.AddComponent<MeshCollider>();
        meshCollider.convex = false;
        meshCollider.isTrigger = true;

        CreateWedgeMesh(); 
    }

    private void CreateWedgeMesh()
    {
        scannerMesh = new Mesh();
        
        int segments = 20;
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 6]; 

        vertices[0] = Vector3.zero;

        float halfAngle = scanAngle * 0.5f;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + ((float)i / segments) * scanAngle;
            float angleRad = angle * Mathf.Deg2Rad;
            float x = Mathf.Sin(angleRad) * scanRadius;
            float z = Mathf.Cos(angleRad) * scanRadius;
            vertices[i + 1] = new Vector3(x, 0, z);
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 6] = 0;
            triangles[i * 6 + 1] = i + 1;
            triangles[i * 6 + 2] = i + 2;

            triangles[i * 6 + 3] = 0;
            triangles[i * 6 + 4] = i + 2;
            triangles[i * 6 + 5] = i + 1;
        }

        scannerMesh.vertices = vertices;
        scannerMesh.triangles = triangles;
        scannerMesh.RecalculateNormals();

        meshFilter.mesh = scannerMesh;
        
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = scannerMesh;
        }
    }

    public void ToggleScanner()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}