using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class LogitUtilityFunction : UtilityFunction
    {
        private String contextValue;
        private double b;
        private double e;

        public LogitUtilityFunction(string contextValue, double b, double e)
        {
            this.contextValue = contextValue;
            this.b = b;
            this.e = e;
        }

        public override double CalculateUtility(Context context)
        {
            double[] valueState = context.GetContextValue(contextValue);
            double currentWeightedValue = CalculateWeightedValueInversed(valueState);

            double utility = CalculateLogit(currentWeightedValue);
            return CheckUtility0To1(utility);
        }

        private double CalculateLogit(double x)
        {
            double number = x / (1 - x);
            return Math.Log(number, e) + b;
        }

    }
}
