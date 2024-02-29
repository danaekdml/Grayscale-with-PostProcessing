using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoToneManager : MonoBehaviour
{
    private static MonoToneManager instance;
    
    public static MonoToneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MonoToneManager>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("MonoToneManagerSingleton");
                    instance = singleton.AddComponent<MonoToneManager>();
                }
            }
            return instance;
        }
    }

    private GameObject[] grayscaleObjects;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();
    private Material grayscaleMaterial;
    private bool isGrayscale = false;
    // private string tagToSet = "Grayscale";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        grayscaleObjects = GameObject.FindGameObjectsWithTag("Grayscale");
        grayscaleMaterial = new Material(Shader.Find("Custom/object grayscale"));

        foreach (GameObject obj in grayscaleObjects)
        {
            StoreOriginalMaterials(obj);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrayscale = !isGrayscale;
            ToggleGrayscaleEffect();
        }
    }
    
    void StoreOriginalMaterials(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material[] originalMaterialsArray = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                originalMaterialsArray[i] = new Material(renderer.materials[i]);
            }
            originalMaterials[obj] = originalMaterialsArray;
        }
    }

    void ToggleGrayscaleEffect()
    {
        foreach (GameObject obj in grayscaleObjects)
        {
            if (isGrayscale)
            {
                ApplyGrayscaleEffect(obj);
            }
            else
            {
                RevertToOriginalMaterials(obj);
            }
        }
    }

    void ApplyGrayscaleEffect(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = Shader.Find("Custom/object grayscale"); 
            }

            renderer.materials = materials;
        }
    }

    void RevertToOriginalMaterials(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && originalMaterials.ContainsKey(obj))
        {
            Material[] originalMaterialsArray = originalMaterials[obj];

            
            if (renderer.materials.Length == originalMaterialsArray.Length)
            {
                renderer.materials = originalMaterialsArray;
            }
        }
    }
}