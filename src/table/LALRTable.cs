using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class LALRTable
    {
        public LALRState states = new LALRState();
        public int initialState = -1;

        public static LALRTable FromXml(XElement xml)
        {
            LALRTable table = new LALRTable();
            table.initialState = xml.Attribute("InitialState").Value.AsInteger();

            foreach (var element in xml.Descendants("LALRState"))
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
