using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class Analyser
    {
        Symbol symbols;
        Production productions;
        LALARTable table;
        List<Metadata> queue;

        public Analyser(XElement table, List<Metadata> queue)
        {
            this.queue = queue;
            this.symbols = Symbol.FromXml(table.Descendants("m_Symbol").FirstOrDefault());
            this.productions = Production.FromXml(table.Descendants("m_Production").FirstOrDefault());
            this.table = LALARTable.FromXml(table.Descendants("LALRTable").FirstOrDefault());
        }

        public void Analyse()
        {

        }
    }
}
