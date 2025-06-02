using System.Collections.Generic;
using UnityEngine;

public class DeskUIManager : MonoBehaviour
{
    public static DeskUIManager instance { get; private set; }

    [Header("Tablet Screens")]
    [SerializeField] public List<GameObject> tabletScreens = new List<GameObject>();
    [SerializeField] public List<GameObject> orderScreens = new List<GameObject>();
    [SerializeField] public Transform dragLayer;
    [SerializeField] public RectTransform blueprintGridContainer;
    [SerializeField] public RectTransform inventoryContainer;

    [Header("Pop-ups")]
    [SerializeField] GameObject coffeeMachinePopUp;
    [SerializeField] GameObject blurLayer;
    [SerializeField] GameObject infoPopup;

    [Header("Player Input")]
    private bool leftClickConsumed;
    public bool LeftClickDown { get; private set; }
    public bool RightClickDown { get; private set; }
    public bool RKeyDown { get; private set; }
    public bool EscapeDown { get; private set; }
    public Vector3 MousePosition { get; private set; }
    [Header("Audio")]
    [HideInInspector] public AudioManager audioManager;

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

        audioManager = GetComponent<AudioManager>();

    }
    private void Start()
    {
        if (tabletScreens.Count > 0)
        {
            OpenOrder(0);
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

        audioManager.PlayAudioClip(audioManager.buttonPress1,transform,1f);

        for (int i = 0; i < tabletScreens.Count; i++)
        {
            if (i != screenIndex)
                tabletScreens[i].SetActive(false);
        }
        tabletScreens[screenIndex].SetActive(true);
    }
    public void OpenOrder(int orderIndex)
    {

        audioManager.PlayAudioClip(audioManager.buttonPress1, transform, 1f);

        for (int i = 0; i < orderScreens.Count; i++)
        {
            if (i != orderIndex)
                orderScreens[i].SetActive(false);
        }
        orderScreens[orderIndex].SetActive(true);
    }
    public void DisplayPopUp()
    {
        blurLayer.SetActive(true);
    }
    public void ClosePopUp()
    {
        blurLayer.SetActive(false);
    }
    public void DisplayComponentPopUp()
    {
        // Make the entire canvas visible
        infoPopup.SetActive(true);
    }

    public void CloseComponentPopUp()
    {
        // Hide the entire canvas
        infoPopup.SetActive(false);
    }
}
