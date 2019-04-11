using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class ComposedUtilityFunction : UtilityFunction
    {
        private String contextValue;
        private List<UtilityFunction> utilityFunctions;
        private double[] changePoints;

        public ComposedUtilityFunction(string contextValue, List<UtilityFunction> utilityFunctions, double[] changePoints)
        {
            this.contextValue = contextValue;
            this.utilityFunctions = utilityFunctions;
            this.changePoints = changePoints;
        }

        public override double CalculateUtility(Context context)
        {
            double[] valueState = context.GetContextValue(contextValue);
            double currentWeightedValue = CalculateWeightedValueInversed(valueState);

            double utility = CalculateComposed(context, currentWeightedValue);
            return CheckUtility0To1(utility);
        }

        private double CalculateComposed(Context context, double x)
        {
            int functionChosen = 0;
            foreach (double n in changePoints)
            {
                if (x < n) break;
                else functionChosen++;
            }
            return utilityFunctions.ElementAt(functionChosen).CalculateUtility(context);
        }
    }
}
