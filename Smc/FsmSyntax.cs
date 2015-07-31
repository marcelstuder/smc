using System.Collections.Generic;

namespace Smc
{
    public class FsmSyntax
    {
        public class Header
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class Transition
        {
            public StateSpec State { get; set; }
            public List<SubTransition> SubTransitions { get; set; } = new List<SubTransition>();
        }

        public class StateSpec
        {
            public string Name { get; set; }
            public string SuperState { get; set; }
            public string EntryAction { get; set; }
            public string ExitAction { get; set; }
            public bool IsAbstractState { get; set; }
        }

        public class SubTransition
        {
            public string Event { get; set; }
            public string NextState { get; set; }
            public List<string> Actions { get; set; } = new List<string>();

            public SubTransition(string evt)
            {
                Event = evt;
            }
        }
    }
}
