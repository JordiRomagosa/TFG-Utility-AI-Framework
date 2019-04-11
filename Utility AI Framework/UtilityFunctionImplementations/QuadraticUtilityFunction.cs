using System;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class QuadraticUtilityFunction : UtilityFunction
    {
        private String contextValue;
        private double k;
        private double c;
        private double b;

        public QuadraticUtilityFunction(string contextValue, double k, double c, double b)
        {
            this.contextValue = contextValue;
            this.k = k;
            this.c = c;
            this.b = b;
        }

        public override double CalculateUtility(Context context)
        {
            double[] valueState = context.GetContextValue(contextValue);
            double currentWeightedValue = CalculateWeightedValueInversed(valueState);

            double utility = CalculateQuadratic(currentWeightedValue);
            return CheckUtility0To1(utility);
        }

        private double CalculateQuadratic(double x)
        {
            return Math.Pow((x - c), k) + b;
        }
    }
}
