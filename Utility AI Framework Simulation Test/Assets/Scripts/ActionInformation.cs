using System.Collections.Generic;

namespace Assets.Scripts
{
    public class ActionInformation
    {
        private Dictionary<Atribute, double> specialModifiers;
        private string actionName;
        private double durationTimeUnits;

        public double DurationSeconds
        {
            get
            {
                return durationTimeUnits;
            }
        }

        public string ActionName
        {
            get
            {
                return actionName;
            }
        }

        public ActionInformation(string actionName, double durationTimeUnits)
        {
            this.specialModifiers = new Dictionary<Atribute, double>();
            this.actionName = actionName;
            this.durationTimeUnits = durationTimeUnits;
        }

        public Dictionary<Atribute, double> GetSpecialModifiers()
        {
            return specialModifiers;
        }

        public void AddSpecialModifier(Atribute atribute, double modifier)
        {
            specialModifiers.Add(atribute, modifier);
        }
    }
}
