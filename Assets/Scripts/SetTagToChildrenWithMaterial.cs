using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTagToChildrenWithMaterial : MonoBehaviour
{
    public string tagToSet = "Grayscale";
    void Start() 
    {
       if (gameObject.tag == "Grayscale")
        {
            SetTagRecursively(transform);
        }
        
    }

    void SetTagRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Renderer renderer = child.GetComponent<Renderer>();

            if (renderer != null && renderer.material != null)
            {
                
                child.gameObject.tag = tagToSet;
                
            }

            
            SetTagRecursively(child);
        }
    }
}
