using System;

namespace Assets.Scripts
{
    public class Atribute
    {
        private String atributeName;
        private double minValue;
        private double maxValue;
        private double currentValue;
        private double valueModifier;

        public Atribute(string atributeName, double minValue, double maxValue, double valueModifier)
        {
            this.atributeName = atributeName;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.currentValue = maxValue;
            this.valueModifier = valueModifier;
        }

        public String AtributeName
        {
            get
            {
                return atributeName;
            }
        }

        public double MinValue
        {
            get
            {
                return minValue;
            }
        }

        public double MaxValue
        {
            get
            {
                return maxValue;
            }
        }

        public double CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        public double ValueModifier
        {
            get
            {
                return valueModifier;
            }
        }

        public void UpdateValue()
        {
            UpdateValue(valueModifier);
        }

        public void UpdateValue(double valueModifier)
        {
            currentValue += valueModifier;
            if (currentValue > maxValue)
                currentValue = maxValue;
            else if (currentValue < minValue)
                currentValue = minValue;
        }

        public Boolean TheValueIsMinimum()
        {
            return currentValue <= minValue;
        }

        public double[] GetValues()
        {
            double[] result = { maxValue, minValue, currentValue };
            return result;
        }
    }
}
