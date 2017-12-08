using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SyntacticAnalysis
{
    class Program
    {
        static List<Metadata> GetLexicalAnalysisResponse()
        {
            string file = File.ReadAllText("LexicalAnalysis/lexical.xml");
            XElement xml = XElement.Parse(file); 
            List<Metadata> queue = Metadata.FromXml(xml);
            return queue;
        }

        static void Main(string[] args)
        {
            List<Metadata> queue = GetLexicalAnalysisResponse();

            foreach(var meta in queue)
                Console.Write(meta.type + " ");
            Console.WriteLine("");
        }
    }
}
