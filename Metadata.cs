using System.Collections.Generic;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    public class Metadata
    {
        public Metadata(string type, string lexval)
        {
            this.type = type;
            this.lexval = lexval;
        }

        public static List<Metadata> FromXml(XElement xml)
        {
             List<Metadata> list = new  List<Metadata>();
             foreach(XElement node in xml.Descendants("metadata"))
             {
                 list.Add(new Metadata(node.Element("type").Value, node.Element("lexval").Value));
             }
             return list;
        }
    
        public string type = "";
        public string lexval = "";
    }
}