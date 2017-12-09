using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class StackState
    {
        public int symbolIndex = -1;
        public int state = -1;

        public StackState(int symbolIndex, int state)
        {
            this.symbolIndex = symbolIndex;
            this.state = state;
        }
    }

    public class Analyser
    {
        Symbol symbols;
        Production productions;
        LALARTable table;
        List<Metadata> queue;
        Stack<StackState> stack = new Stack<StackState>();

        public Analyser(XElement table, List<Metadata> queue)
        {
            this.queue = queue;
            this.symbols = Symbol.FromXml(table.Descendants("m_Symbol").FirstOrDefault());
            this.productions = Production.FromXml(table.Descendants("m_Production").FirstOrDefault());
            this.table = LALARTable.FromXml(table.Descendants("LALRTable").FirstOrDefault());
        }

        public void Analyse()
        {
            int currentState = table.initialState;
            foreach(var element in queue)
            {
                Do(element, ref currentState);
            }
        }

        public void Do(Metadata element, ref int currentState)
        {
            int symbolIndex = symbols.FirstOrDefault(r => r.Value.name == element.type).Key;

            LALRStateElement state = table.states[currentState];
            LALRAction action = state.actions.FirstOrDefault(r => r.symbolIndex == symbolIndex);

            Action toDo = ActionBuilder.FromId(action.action);

            if (toDo == Action.shiftTo)
            {
                stack.Push(new StackState(symbolIndex, currentState));
                currentState = action.value;
            }
            else if (toDo == Action.goTo)
            {
                currentState = action.value;
            }
            else if (toDo == Action.reduceProduction)
            {
                ProductionElement production = productions[action.value];

                StackState stackState = null;
                for (int i = 0; i < production.productionSymbols.Count(); i++)
                {
                    stackState = stack.Pop();
                }

                stack.Push(new StackState(production.nonTerminalIndex, currentState));
                Do(element, ref currentState);
            }
        }
    }
}
