using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    public static ComponentManager instance { get; private set; }

    [Header("Components")]
    public List<ComponentData> components = new List<ComponentData>();
    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Generate product IDs
        for (int i = 0; i < components.Count; i++)
        {
            components[i].componentID = i;
        }

    }
}
