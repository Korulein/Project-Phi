using TMPro;
using UnityEngine;

public class RequirementUI : MonoBehaviour
{
    public TMP_Text label;
    private Requirement data;
    public bool isPurchased;

    public void Setup(Requirement requirement)
    {
        data = requirement;
        label.text = $"{requirement.name}: {requirement.amount}";
        label.enabled = true;
        isPurchased = false;
    }

    public bool Evaluate()
    {
        float totalHeat = BlueprintManager.instance.GetTotalProducedHeat();
        return Evaluate(totalHeat);
    }

    public bool Evaluate(float totalHeat)
    {
        if (data == null)
        {
            Debug.LogError("Requirement data is null!");
            return false;
        }

        if (data.type == Requirement.RequirementType.Component)
        {
            return BlueprintManager.instance.CountComponentsWithTag(data.componentTag) >= data.amount;
        }
        else if (data.type == Requirement.RequirementType.NumericValue)
        {
            if (data.componentTag == "Heating Element")
            {
                return totalHeat >= data.numericValue;
            }
            else
            {
                return BlueprintManager.instance.CheckNumericRequirement(data.componentTag, (int)data.numericValue);
            }
        }

        return false;
    }

    public void SetColor(Color color)
    {
        if (label != null)
        {
            label.color = color;
            label.enabled = true;
        }
    }

    private void UpdateVisual()
    {
        bool isMet = Evaluate();
        if (isMet)
        {
            SetColor(Color.green);
        }
        else if (isPurchased)
        {
            SetColor(Color.yellow);
        }
        else
        {
            SetColor(Color.red);
        }
    }

    private void UpdatePurchasedVisual()
    {
        isPurchased = true;
        SetColor(Color.yellow);
    }

    public string GetCategoryName()
    {
        if (data != null)
            return data.componentTag;
        return null;
    }

    public void OnComponentPlaced(string placedCategory)
    {
        if (data == null)
        {
            Debug.LogError("RequirementUI data is null in OnComponentPlaced!");
            return;
        }

        if (data.componentTag == placedCategory)
        {
            bool isMet = Evaluate();

            if (isMet)
            {
                SetColor(Color.green);
            }
            else if (isPurchased)
            {
                SetColor(Color.yellow);
            }
            else
            {
                SetColor(Color.red);
            }
        }
    }

    public void OnComponentPurchased(string placedCategory)
    {
        if (data == null)
        {
            Debug.LogError("RequirementUI data is null in OnComponentPurchased!");
            return;
        }

        if (data.componentTag == placedCategory)
        {
            UpdatePurchasedVisual();
        }
    }

    public void OnComponentRemoved(string removedCategory)
    {
        if (data == null) return;

        if (data.componentTag == removedCategory)
        {
            bool isMet = Evaluate();

            if (isMet)
                SetColor(Color.green);
            else if (isPurchased)
                SetColor(Color.yellow);
            else
                SetColor(Color.red);
        }
    }


    public bool HasData()
        {
            return data != null;
        }

        public void ResetData()
        {
            data = null;
            isPurchased = false;
            label.text = "";
            label.enabled = false;
        }
}