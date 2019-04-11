using System;

namespace UtilityAI
{
    public class Action
    {
        private double inertia;
        private UtilityFunction utilityFunction;

        public Action(double inertia, UtilityFunction utlityFunction)
        {
            this.inertia = inertia;
            this.utilityFunction = utlityFunction;
        }

        public Action(UtilityFunction utilityFunction) : this(0, utilityFunction) { }

        public double Inertia { get => inertia; set => inertia = value; }

        public double GetUtility(Context context)
        {
            return utilityFunction.CalculateUtility(context);
        }
    }
}
