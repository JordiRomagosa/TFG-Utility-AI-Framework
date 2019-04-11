using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject objectAtribute1;
    public GameObject objectAtribute2;
    public GameObject objectAtribute3;
    public GameObject objectAtribute4;
    public GameObject objectAtribute5;
    public GameObject objectAtribute6;

    public Text happiness;
    public Text time;
    public Text currentActivity;

    public GameObject deathPanel;

    private List<UIAtribute> atributes;

    void Start()
    {
        atributes = new List<UIAtribute>
        {
            objectAtribute1.GetComponent<UIAtribute>(),
            objectAtribute2.GetComponent<UIAtribute>(),
            objectAtribute3.GetComponent<UIAtribute>(),
            objectAtribute4.GetComponent<UIAtribute>(),
            objectAtribute5.GetComponent<UIAtribute>(),
            objectAtribute6.GetComponent<UIAtribute>()
        };
        deathPanel.gameObject.SetActive(false);
    }

    public void UpdateMainInformation(string happiness, string time, string currentActivity)
    {
        this.happiness.text = happiness;
        this.time.text = time;
        this.currentActivity.text = currentActivity;
    }

    public void SetAtributeInitialTexts(string atributeName, string currentValue, string maxValue, string minValue)
    {
        foreach (UIAtribute atribute in atributes)
            if (atribute.atributeName.text.Equals("Value"))
            {
                atribute.SetInitialTexts(atributeName, currentValue, maxValue, minValue);
                break;
            }
    }

    public void UpdateAtributeCurrentText(string atributeName, double currentValue, double valueModifier)
    {
        foreach (UIAtribute atribute in atributes)
            if (atribute.atributeName.text.Equals(atributeName))
                if (valueModifier > 0)
                    atribute.UpdateCurrentVal(currentValue + " (+" + valueModifier + ")");
                else
                    atribute.UpdateCurrentVal(currentValue + " (" + valueModifier + ")");
    }

    public void ShowDeathPanel()
    {
        deathPanel.gameObject.SetActive(true);
    }
}
