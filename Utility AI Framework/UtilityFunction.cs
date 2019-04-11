using System;

namespace UtilityAI
{
    public abstract class UtilityFunction
    {
        public abstract double CalculateUtility(Context context);

        protected double CalculateWeightedValueInversed(double[] values)
        {
            double maxValue = values[0];
            double minValue = values[1];
            double currentValue = values[2];
            double weightedValue = (currentValue - minValue) / (maxValue - minValue);
            return -weightedValue + 1;
        }

        protected double CheckUtility0To1(double utility)
        {
            if (utility > 1) return 1;
            else if (utility < 0) return 0;
            return utility;
        }
    }
}
