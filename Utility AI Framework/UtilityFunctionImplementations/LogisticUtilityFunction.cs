using System;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class LogisticUtilityFunction : UtilityFunction
    {
        private String contextValue;
        private double c;
        private double e;

        public LogisticUtilityFunction(string contextValue, double c, double e)
        {
            this.contextValue = contextValue;
            this.c = c;
            this.e = e;
        }

        public override double CalculateUtility(Context context)
        {
            double[] valueState = context.GetContextValue(contextValue);
            double currentWeightedValue = CalculateWeightedValueInversed(valueState);

            double utility = CalculateLogistic(currentWeightedValue);
            return CheckUtility0To1(utility);
        }

        private double CalculateLogistic(double x)
        {
            double power = -x + c;
            return 1 / (1 + Math.Pow(e, power));
        }
    }
}
