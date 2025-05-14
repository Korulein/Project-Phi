using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    public Tooltip tooltip;
    private void Awake()
    {
        instance = this;
    }
    public void ShowTooltip(string content)
    {
        instance.tooltip.SetText(content);
        instance.tooltip.gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
