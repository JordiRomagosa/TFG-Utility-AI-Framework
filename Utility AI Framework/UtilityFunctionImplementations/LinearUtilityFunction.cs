using System;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class LinearUtilityFunction : UtilityFunction
    {
        private String contextValue;
        private double m;
        private double b;

        public LinearUtilityFunction(string contextValue, double m, double b)
        {
            this.contextValue = contextValue;
            this.m = m;
            this.b = b;
        }

        public override double CalculateUtility(Context context)
        {
            double[] valueState = context.GetContextValue(contextValue);
            double currentWeightedValue = CalculateWeightedValueInversed(valueState);

            double utility = CalculateLinear(currentWeightedValue);
            return CheckUtility0To1(utility);
        }

        private double CalculateLinear(double x)
        {
            return m * x + b;
        }
    }
}
