    using TMPro;
    using UnityEngine;

    public class RequirementUI : MonoBehaviour
    {
        public TMP_Text label;
        private Requirement data;

        public void Setup(Requirement requirement)
        {
            data = requirement;
            label.text = $"{requirement.name}: {requirement.amount}";
            label.enabled = true;
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
            SetColor(isMet ? Color.green : Color.red);
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
                UpdateVisual();
            }
        }

        public bool HasData()
        {
            return data != null;
        }

        public void ResetData()
        {
            data = null;
            label.text = "";
            label.enabled = false;
        }
    }