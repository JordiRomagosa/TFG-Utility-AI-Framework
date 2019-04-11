using System;

namespace UtilityAI
{
    public interface Context
    {
        double[] GetContextValue(String valueName);
    }
}
