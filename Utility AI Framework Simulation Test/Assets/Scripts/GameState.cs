using System.Collections.Generic;
using UtilityAI;

namespace Assets.Scripts
{
    public class GameState : Context
    {
        private List<Atribute> atributes;

        public List<Atribute> Atributes
        {
            get
            {
                return atributes;
            }
        }

        public GameState(List<Atribute> atributes)
        {
            this.atributes = atributes;
        }
        public void UpdateGameState()
        {
            UpdateGameState(new Dictionary<Atribute, double>());
        }

        public void UpdateGameState(Dictionary<Atribute, double> specialAtributesValues)
        {
            foreach (Atribute atribute in atributes)
            {
                if (specialAtributesValues.ContainsKey(atribute))
                    atribute.UpdateValue(specialAtributesValues[atribute]);
                else
                    atribute.UpdateValue();
            }
        }

        public bool CheckCharacterDies()
        {
            foreach (Atribute atribute in atributes)
            {
                if (atribute.TheValueIsMinimum())
                    return true;
            }
            return false;
        }

        public double[] GetContextValue(string valueName)
        {
            Atribute valueAtribute = null;
            foreach (Atribute atribute in atributes)
            {
                if (atribute.AtributeName.Equals(valueName))
                {
                    valueAtribute = atribute;
                    break;
                }
            }
            if (valueAtribute == null)
                return new double[] { 1, 0, 0 };
            return valueAtribute.GetValues();
        }
    }
}
