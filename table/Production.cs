using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class ProductionElement
    {
        public int nonTerminalIndex = -1;
        public List<int> productionSymbols = new List<int>();
    }

    public class Production : Dictionary<int, ProductionElement>
    {
        public static Production FromXml(XElement xml)
        {
            Production production = new Production();

            foreach (var element in xml.Descendants("Production"))
            {
                int index = element.Attribute("Index").Value.AsInteger();

                ProductionElement productionElement = new ProductionElement()
                {
                    nonTerminalIndex = element.Attribute("NonTerminalIndex").Value.AsInteger(),
                };

                foreach(var productionSymbol in element.Descendants("ProductionSymbol"))
                    productionElement.productionSymbols.Add(productionSymbol.Attribute("SymbolIndex").Value.AsInteger());
                
                production[index] = productionElement;
            }

            return production;
        }
    }
}
