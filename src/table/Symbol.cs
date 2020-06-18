using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class SymbolElement
    {
        public SymbolElement(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public string name = "";
        public string type = "";
    }

    public class Symbol : Dictionary<int, SymbolElement>
    {
        public static Symbol FromXml(XElement xml)
        {
            Symbol symbol = new Symbol();

            foreach (var element in xml.Descendants("Symbol"))
            {
                int index = element.Attribute("Index").Value.AsInteger();
                string name = element.Attribute("Name").Value;
                string type = element.Attribute("Type").Value;
                symbol[index] = new SymbolElement(name, type);
            }
            return symbol;
        }
    }
}
