using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityAI
{
    public class BestActionCalculator
    {
        public const int BEST_ACTION = 0;
        public const int WEIGHTED_RANDOM = 1;
        public const int RANDOM_BEST_3 = 2;

        public static Action CalculateBestAction(List<Action> actions, Action currentAction, Context context, int chooseMethod, Boolean useInertia)
        {
            switch (chooseMethod)
            {
                case BEST_ACTION:
                    return BestActionMethod(actions, currentAction, context, useInertia);
                case WEIGHTED_RANDOM:
                    return WeightedRandomMethod(actions, currentAction, context, useInertia);
                case RANDOM_BEST_3:
                    return RandomBest3Method(actions, currentAction, context, useInertia);
                default:
                    return DefaultActionChoose(actions, currentAction);
            }
        }

        private static Action DefaultActionChoose(List<Action> actions, Action currentAction)
        {
            if (currentAction != null) return currentAction;

            int randomIndex = new Random().Next(actions.Count);
            return actions[randomIndex];
        }

        private static double ApplyInertiaToUtility(double baseUtility, double inertiaValue)
        {
            if (inertiaValue > 1) inertiaValue = 1;
            else if (inertiaValue < 0) inertiaValue = 0;
            return baseUtility + baseUtility * inertiaValue;
        }

        private static Action BestActionMethod(List<Action> actions, Action currentAction, Context context, Boolean useInertia)
        {
            Action bestAction = actions[0];
            actions.RemoveAt(0);
            double bestUtility = bestAction.GetUtility(context);
            if (useInertia && bestAction.Equals(currentAction))
                bestUtility = ApplyInertiaToUtility(bestUtility, bestAction.Inertia);
            
            foreach (Action action in actions){
                double currentActionUtility = action.GetUtility(context);
                if (useInertia && action.Equals(currentAction))
                    currentActionUtility = ApplyInertiaToUtility(currentActionUtility, action.Inertia);
                if (currentActionUtility > bestUtility)
                {
                    bestAction = action;
                    bestUtility = currentActionUtility;
                }
            }
            return bestAction;
        }

        private static Action WeightedRandomMethod(List<Action> actions, Action currentAction, Context context, Boolean useInertia)
        {
            List<KeyValuePair<Action, double>> actionUtilityPairs = new List<KeyValuePair<Action, double>>();

            double accumulatedUtility = 0;
            foreach (Action action in actions)
            {
                double currentActionUtility = action.GetUtility(context);
                if (useInertia && action.Equals(currentAction))
                    currentActionUtility = ApplyInertiaToUtility(currentActionUtility, action.Inertia);
                accumulatedUtility += currentActionUtility;
                actionUtilityPairs.Add(new KeyValuePair<Action, double>(action, accumulatedUtility));
            }
            double random = new Random().NextDouble() * accumulatedUtility;
            foreach (KeyValuePair<Action, double> actionUtilityPair in actionUtilityPairs)
            {
                if (random < actionUtilityPair.Value)
                    return actionUtilityPair.Key;
            }
            return currentAction;
        }

        private static Action RandomBest3Method(List<Action> actions, Action currentAction, Context context, Boolean useInertia)
        {
            Dictionary<Action, double> actionUtilityPairs = new Dictionary<Action, double>();
            foreach (Action action in actions) {
                double currentActionUtility = action.GetUtility(context);
                if (useInertia && action.Equals(currentAction))
                    currentActionUtility = ApplyInertiaToUtility(currentActionUtility, action.Inertia);
                actionUtilityPairs.Add(action, currentActionUtility);
            }
            var items = from pair in actionUtilityPairs orderby pair.Value descending select pair;
            Dictionary<Action, double> bestActionUtilityPairs = new Dictionary<Action, double>();
            foreach (KeyValuePair<Action, double> pair in items)
            {
                if (bestActionUtilityPairs.Count > 3)
                    break;
                bestActionUtilityPairs.Add(pair.Key, pair.Value);
            }
            return bestActionUtilityPairs.ElementAt(new Random().Next(0, bestActionUtilityPairs.Count)).Key;
        }
    }
}
