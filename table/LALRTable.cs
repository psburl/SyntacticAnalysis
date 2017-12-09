using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class LALARTable
    {
        LALRState states = new LALRState();

        public static LALARTable FromXml(XElement xml)
        {
            LALARTable table = new LALARTable();

            foreach(var element in xml.Descendants("LALRState"))
            {
                int index = element.Attribute("Index").Value.AsInteger();

                LALRStateElement stateElement = new LALRStateElement();

                foreach(var actionElemnt in element.Descendants("LALRAction"))
                {
                    LALRAction action = new LALRAction();
                    action.symbolIndex = actionElemnt.Attribute("SymbolIndex").Value.AsInteger();
                    action.action = actionElemnt.Attribute("Action").Value.AsInteger();
                    action.value = actionElemnt.Attribute("Value").Value.AsInteger();
                    stateElement.actions.Add(action);
                }


                table.states[index] = stateElement;
            }

            return table;
        }
    }
}
