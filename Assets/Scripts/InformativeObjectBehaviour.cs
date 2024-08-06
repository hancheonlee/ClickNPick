using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformativeObjectBehaviour : MonoBehaviour
{

    public bool selected;

    private Material originalMaterial;
    public Material selectedMaterial;

    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        originalMaterial = objectRenderer.material;
    }

    private void Update()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (selected)
        {
            objectRenderer.material = selectedMaterial;
        }
        else
        {
            objectRenderer.material = originalMaterial;
        }    
    }
}
