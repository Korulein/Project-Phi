using System.Collections.Generic;
using UnityEngine;

public class DeskUIManager : MonoBehaviour
{
    public static DeskUIManager instance { get; private set; }

    [Header("Tablet Screens")]
    [SerializeField] public List<GameObject> tabletScreens = new List<GameObject>();
    [SerializeField] public Transform dragLayer;
    [SerializeField] public RectTransform blueprintGridContainer;
    [SerializeField] public RectTransform inventoryContainer;
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
    public bool IsCursorOverBlueprint(Vector2 mousePosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(blueprintGridContainer, mousePosition);
    }
    public bool IsCursorOverInventory(Vector2 mousePosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryContainer, mousePosition);
    }
}
