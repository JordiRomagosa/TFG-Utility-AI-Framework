using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityAI.UtilityFunctionImplementations
{
    public class ConstantUtilityFunction : UtilityFunction
    {
        double constantValue;

        public ConstantUtilityFunction(double constantValue)
        {
            this.constantValue = constantValue;
        }

        public override double CalculateUtility(Context context)
        {
            double utility = CalculateConstant();
            return CheckUtility0To1(utility);
        }

        private double CalculateConstant()
        {
            return constantValue;
        }
    }
}
