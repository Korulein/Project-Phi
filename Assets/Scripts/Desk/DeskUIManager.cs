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

    [Header("Pop-ups")]
    [SerializeField] GameObject coffeeMachinePopUp;

    [Header("Player Input")]
    private bool leftClickConsumed;
    public bool LeftClickDown { get; private set; }
    public bool RightClickDown { get; private set; }
    public bool RKeyDown { get; private set; }
    public bool EscapeDown { get; private set; }
    public Vector3 MousePosition { get; private set; }
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
    private void Update()
    {
        MousePosition = Input.mousePosition;
        LeftClickDown = Input.GetMouseButtonDown(0);
        RightClickDown = Input.GetMouseButtonDown(1);
        EscapeDown = Input.GetKeyDown(KeyCode.Escape);
        RKeyDown = Input.GetKeyDown(KeyCode.R);
    }
    void LateUpdate()
    {
        // Reset consumption state
        leftClickConsumed = false;
    }
    public bool TryConsumeLeftClick()
    {
        if (LeftClickDown && !leftClickConsumed)
        {
            leftClickConsumed = true;
            return true;
        }
        return false;
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
    public void DisplayPopUp()
    {
        coffeeMachinePopUp.SetActive(true);
    }
    public void ClosePopUp()
    {
        coffeeMachinePopUp.SetActive(false);
    }
}
