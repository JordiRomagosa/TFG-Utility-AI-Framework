using UtilityAI;
using UtilityAI.UtilityFunctionImplementations;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private const double UPDATE_SECONDS = 1;
    private const double PAUSE_BETWEEN_ACTIONS_SECONDS = UPDATE_SECONDS;

    private double nextUpdate;
    private double nextActionChange;
    private double lastActionChange;
    private GameState gameState;
    private Dictionary<Action, ActionInformation> actionInformation;
    private Action currentAction;

    public UIController UIcontroller;

	void Start ()
    {
        nextUpdate = Mathf.FloorToInt(Time.time) + UPDATE_SECONDS;
        actionInformation = new Dictionary<Action, ActionInformation>();
        List<Atribute> atributes = BuildActionsAndAtributes();
        gameState = new GameState(atributes);
        PrepareActionToMake();
        ShowUIInitialTexts();
        UpdateMainInformation();
        UpdateAtributeTexts();
    }

    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = nextUpdate + UPDATE_SECONDS;
            UpdateFixedInterval();
        }
    }

    private void UpdateFixedInterval()
    {
        if (gameState.CheckCharacterDies())
        {
            UIcontroller.ShowDeathPanel();
            this.enabled = false;
        }
        if (Time.time >= lastActionChange + PAUSE_BETWEEN_ACTIONS_SECONDS)
        {
            gameState.UpdateGameState(actionInformation[currentAction].GetSpecialModifiers());
            UpdateMainInformation();
            UpdateAtributeTexts();
        }
        else
        {
            gameState.UpdateGameState();
            UpdateMainInformationIdle();
            UpdateAtributeTextsIdle();
        }
        if (Time.time >= nextActionChange)
            PrepareActionToMake();
    }

    private void PrepareActionToMake()
    {
        List<Action> actions = new List<Action>(actionInformation.Keys);
        Action bestAction = BestActionCalculator.CalculateBestAction(actions, currentAction, gameState, BestActionCalculator.BEST_ACTION, false);
        currentAction = bestAction;
        lastActionChange = nextUpdate;
        nextActionChange = nextUpdate + PAUSE_BETWEEN_ACTIONS_SECONDS + actionInformation[currentAction].DurationSeconds;
    }

    private void ShowUIInitialTexts()
    {
        foreach (Atribute atribute in gameState.Atributes)
            UIcontroller.SetAtributeInitialTexts(atribute.AtributeName, atribute.CurrentValue.ToString(),
                atribute.MaxValue.ToString(), atribute.MinValue.ToString());
    }

    private void UpdateMainInformation()
    {
        string currentActionString = actionInformation[currentAction].ActionName;
        UIcontroller.UpdateMainInformation(GetAverageHappiness(), GetTimeFormatted(), currentActionString);
    }

    private void UpdateMainInformationIdle()
    {
        UIcontroller.UpdateMainInformation(GetAverageHappiness(), GetTimeFormatted(), "Idle");
    }

    private void UpdateAtributeTexts()
    {
        foreach (Atribute atribute in gameState.Atributes)
        {
            Dictionary<Atribute, double> atributeSpecialModifiers = actionInformation[currentAction].GetSpecialModifiers();
            if (atributeSpecialModifiers.ContainsKey(atribute))
                UIcontroller.UpdateAtributeCurrentText(atribute.AtributeName, atribute.CurrentValue, atributeSpecialModifiers[atribute]);
            else
                UIcontroller.UpdateAtributeCurrentText(atribute.AtributeName, atribute.CurrentValue, atribute.ValueModifier);
        }
    }

    private void UpdateAtributeTextsIdle()
    {
        foreach (Atribute atribute in gameState.Atributes)
        {
            UIcontroller.UpdateAtributeCurrentText(atribute.AtributeName, atribute.CurrentValue, atribute.ValueModifier);
        }
    }

    private string GetAverageHappiness()
    {
        int count = 0;
        double sum = 0;
        foreach (Atribute atribute in gameState.Atributes)
        {
            sum += (atribute.CurrentValue - atribute.MinValue) / (atribute.MaxValue - atribute.MinValue);
            count++;
        }
        int average = (int) (sum / count * 100);
        return average + "/100";
    }

    private string GetTimeFormatted()
    {
        int totalSeconds = Mathf.FloorToInt(Time.time);
        int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;
        if (seconds < 10)
            return minutes + ":0" + seconds;
        return minutes + ":" + seconds;
    }

    List<Atribute> BuildActionsAndAtributes()
    {
        Atribute fullness = new Atribute("Fullness", 0, 100, -3);
        Atribute awake = new Atribute("Awake", 0, 100, -1);
        Atribute relax = new Atribute("Relax", 0, 100, -1);
        Atribute clean = new Atribute("Clean", 0, 100, -1);
        Atribute money = new Atribute("Money", 0, 100, -2);
        Atribute energy = new Atribute("Energy", 0, 100, +1);

        Action action = new Action(new LogisticUtilityFunction("Fullness", 1, System.Math.E));
        ActionInformation information = new ActionInformation("Eat", 4 * UPDATE_SECONDS);
        information.AddSpecialModifier(fullness, 10);
        actionInformation.Add(action, information);

        action = new Action(new LogitUtilityFunction("Awake", 1, System.Math.E));
        information = new ActionInformation("Sleep", 8 * UPDATE_SECONDS);
        information.AddSpecialModifier(awake, 5);
        information.AddSpecialModifier(relax, 1);
        actionInformation.Add(action, information);

        action = new Action(new LinearUtilityFunction("Money", 0.8, 0.2));
        information = new ActionInformation("Work", 6 * UPDATE_SECONDS);
        information.AddSpecialModifier(money, 8);
        information.AddSpecialModifier(relax, -4);
        actionInformation.Add(action, information);

        List<UtilityFunction> composedFunctions = new List<UtilityFunction>
        {
            new ConstantUtilityFunction(0),
            new LinearUtilityFunction("Stress", 2, -1.2)
        };
        double[] cutPoints = new double[] { 0.6 };
        action = new Action(new ComposedUtilityFunction("Relax", composedFunctions, cutPoints));
        information = new ActionInformation("Exercise", 4 * UPDATE_SECONDS);
        information.AddSpecialModifier(relax, 15);
        information.AddSpecialModifier(clean, -5);
        information.AddSpecialModifier(energy, -10);
        actionInformation.Add(action, information);

        action = new Action(new ConstantUtilityFunction(0.3));
        information = new ActionInformation("Watch TV", 2 * UPDATE_SECONDS);
        information.AddSpecialModifier(relax, 10);
        actionInformation.Add(action, information);

        action = new Action(new QuadraticUtilityFunction("Clean", 5, 0, 0));
        information = new ActionInformation("Shower", 3 * UPDATE_SECONDS);
        information.AddSpecialModifier(clean, 40);
        actionInformation.Add(action, information);

        List<Atribute> atributes = new List<Atribute>
        {
            fullness,
            awake,
            relax,
            clean,
            money,
            energy
        };

        return atributes;
    }
}
