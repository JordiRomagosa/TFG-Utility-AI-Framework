using UnityEngine;
using UnityEngine.UI;

public class UIAtribute : MonoBehaviour {

    public Text atributeName;
    public Text atributeCurrentVal;
    public Text atributeMax;
    public Text atributeMin;

    public void SetInitialTexts(string atributeName, string atributeCurrentVal, string atributeMax, string atributeMin)
    {
        this.atributeName.text = atributeName;
        this.atributeCurrentVal.text = atributeCurrentVal;
        this.atributeMax.text = atributeMax;
        this.atributeMin.text = atributeMin;
    }

    public void UpdateCurrentVal(string atributeCurrentVal)
    {
        this.atributeCurrentVal.text = atributeCurrentVal;
    }
}
