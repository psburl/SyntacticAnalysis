using System;
using System.Collections.Generic;
using System.Text;

namespace SyntacticAnalysis
{
    public enum Action
    {
        none,
        shiftTo = 1,
        reduceProduction,
        goTo
    };

    public class ActionBuilder
    {
        public static Action FromId(int id)
        {
            switch (id)
            {
                case 1: return Action.shiftTo;
                case 2: return Action.reduceProduction;
                case 3: return Action.goTo;
                default: return Action.none;
            }
        }
    }
}
