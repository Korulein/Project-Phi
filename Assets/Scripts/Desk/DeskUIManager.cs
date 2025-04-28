using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskUIManager : MonoBehaviour
{
    public static DeskUIManager instance { get; private set; }

    [Header("Tablet Screens")]
    [SerializeField] public List<GameObject> tabletScreens = new List<GameObject>();

    [Header("Tablet Buttons")]
    [SerializeField] Button emailButton;
    [SerializeField] Button supplierButton;
    [SerializeField] Button partsInventoryButton;

    //[Header("Pop Ups")]
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
    public void OpenScreen(int screenIndex)
    {
        for (int i = 0; i < tabletScreens.Count; i++)
        {
            if (i != screenIndex)
                tabletScreens[i].SetActive(false);
        }
        tabletScreens[screenIndex].SetActive(true);
    }
}
