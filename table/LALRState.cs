using System;
using System.Collections.Generic;
using System.Text;

namespace SyntacticAnalysis
{
    public class LALRAction
    {
        public int symbolIndex = -1;
        public int action = -1;
        public int value = -1;
    }

    public class LALRStateElement
    {
        public List<LALRAction> actions = new List<LALRAction>();
    }

    public class LALRState : Dictionary<int, LALRStateElement>
    {

    }
}
