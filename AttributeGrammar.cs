using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntacticAnalysis
{
    class AttributeGrammar
    {
        public static List<AttributeGrammar> FromGrammar(string grammar, LALRTable table, Symbol symbols, Production productions)
        {
            List<AttributeGrammar> list = new List<AttributeGrammar>();

            string currentRule = "";
            foreach (var line in grammar.Replace("\r", "").Split('\n'))
            {
                var element = line;

                if (Regex.Match(line, "<[^>]+> ::= ").Success)
                {
                    var parts = element.Split("::=");
                    currentRule = parts[0].Replace("<", "").Replace(">", "").Replace("'", "").Trim();
                    element = parts[1];
                }
                else
                {
                    element = element.Replace("|", "");
                }

                AttributeGrammar attribute = new AttributeGrammar();
                attribute.rule = currentRule;
                attribute.production = element;
                foreach (Match value in Regex.Matches(element, "<[^>]+>|'[^']+'"))
                {
                    int symbolIndex = symbols.FirstOrDefault(r => r.Value.name == value.Value.Replace("<", "").Replace(">", "").Replace("'", "")).Key;
                    attribute.indexes.Add(symbolIndex);
                }

                attribute.hash = string.Join(" ", attribute.indexes);

                var match = Regex.Match(element, "{[^}]+}");
                if (match.Success)
                    attribute.actions = match.Value.Replace("{", "").Replace("}", "").Split(';').ToList();
                
                list.Add(attribute);
            }

            return list;
        }

        public string production = "";
        public List<string> actions = new List<string>();
        public List<int> indexes = new List<int>();
        public string hash = "";
        public string rule = "";
    }
}
