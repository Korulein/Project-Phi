using System.Collections.Generic;
using UnityEngine;

public class SuppliersManager : MonoBehaviour
{
    public static SuppliersManager instance { get; private set; }
    [Header("Suppliers")]
    [SerializeField] public List<GameObject> supplierScreens = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenSupplier(int supplierIndex)
    {
        for (int i = 0; i < supplierScreens.Count; i++)
        {
            if (i != supplierIndex)
                supplierScreens[i].SetActive(false);
        }
        supplierScreens[supplierIndex].SetActive(true);
        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);
    }
}
