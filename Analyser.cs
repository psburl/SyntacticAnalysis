using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class StackState
    {
        public int symbolIndex = -1;
        public int state = -1;
        public Metadata metadata;

        public StackState(int symbolIndex, int state, Metadata metadata)
        {
            this.symbolIndex = symbolIndex;
            this.state = state;
            this.metadata = metadata;
        }
    }

    public class Analyser
    {
        Symbol symbols;
        Production productions;
        LALRTable table;
        List<Metadata> queue;
        Stack<StackState> stack = new Stack<StackState>();
        List<AttributeGrammar> attributeGrammar;

        Metadata last = null;
        int line = 0;

        public Analyser(XElement table, List<Metadata> queue)
        {
            this.queue = queue;
            this.symbols = Symbol.FromXml(table.Descendants("m_Symbol").FirstOrDefault());
            this.productions = Production.FromXml(table.Descendants("m_Production").FirstOrDefault());
            this.table = LALRTable.FromXml(table.Descendants("LALRTable").FirstOrDefault());
            this.attributeGrammar = AttributeGrammar.FromGrammar(File.ReadAllText("gold/glc.txt"), this.table, this.symbols, this.productions);
        }

        public void Analyse()
        {
            int currentState = table.initialState;
            foreach(var element in queue)
            {
                if(element.lexval == "\n"){
                    line++;
                    continue;
                }

                Do(element, ref currentState);
            }
        }

        public List<Metadata> GetTable()
        {
            return queue;
        }

        public void Do(Metadata element, ref int currentState)
        {
            int symbolIndex = symbols.FirstOrDefault(r => r.Value.name == element.type).Key;
            element.symbolIndex = symbolIndex;

            LALRStateElement state = table.states[currentState];
            LALRAction action = state.actions.FirstOrDefault(r => r.symbolIndex == symbolIndex);

            if(action == null)
            {
                int eofIndex = symbols.FirstOrDefault(r => r.Value.name == "EOF").Key;

                action = state.actions.FirstOrDefault(r => r.symbolIndex == eofIndex);

                if (action != null)
                {
                    Metadata metadata = new Metadata("EOF", "");
                    Do(metadata, ref currentState);
                    Do(element, ref currentState);
                    return;
                }
                else
                {
                    string complement = "";
                    if(this.last == null)
                        complement = "at start of code";
                    else
                        complement = $"after {last?.lexval??""}";

                    throw new Exception($"Unexpected token {element.lexval} {complement} at line {line}");
                }
            }

            Action toDo = ActionBuilder.FromId(action.action);

            if (toDo == Action.shiftTo)
            {
                stack.Push(new StackState(symbolIndex, currentState, element));
                currentState = action.value;
            }
            else if (toDo == Action.goTo)
            {
                currentState = action.value;
            }
            else if (toDo == Action.reduceProduction)
            {
                ProductionElement production = productions[action.value];

                List<int> reduction = new List<int>();
                List<Metadata> context = new List<Metadata>();
                for (int i = 0; i < production.productionSymbols.Count(); i++)
                {
                    StackState stackState = stack.Pop();
                    currentState = stackState.state;
                    reduction.Add(stackState.symbolIndex);
                    context.Add(stackState.metadata);
                }
                context.Reverse();
                reduction.Reverse();

                string hash = string.Join(" ", reduction);

                AttributeGrammar attribute = attributeGrammar.FirstOrDefault(r => r.hash == hash);

                Metadata metadata = new Metadata(symbols[production.nonTerminalIndex].name, "");
                context.Add(metadata);

                foreach (var attributeAction in attribute.actions)
                {
                    if(Regex.Match(attributeAction, "[^=]+ = [^.]+").Success)
                    {
                        var parts = attributeAction.Split('=');
                        var ref1 = context.FirstOrDefault(r => r.type == parts[0].Split('.')[0].Trim());
                        var ref2 = context.FirstOrDefault(r => r.type == parts[1].Split('.')[0].Trim());

                        var field1 = ref1.GetType().GetField(parts[0].Split('.')[1].Trim());
                        var field2 = ref2.GetType().GetField(parts[1].Split('.')[1].Trim());
                        var value = field2.GetValue(ref2);
                        field1.SetValue(ref1, value);
                    }
                }

                
                stack.Push(new StackState(production.nonTerminalIndex, currentState, metadata));
                Do(metadata, ref currentState);
                Do(element, ref currentState);
            }
            else if(toDo == Action.accept)
            {
                currentState = action.value;
            }

            last = element;
        }
    }
}
