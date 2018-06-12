using System.Collections.Generic;

namespace Multiverse.Runtime
{
    public class FiniteStateMachine
    {
        private IState prevState;
        private IState curState;

        public Dictionary<string, IState> states = new Dictionary<string, IState>();

        public void Register(string name, IState state)
        {
            if (!states.ContainsKey(name))
            {
                states.Add(name, state);
            }
        }

        public void Update()
        {
            if (curState != null)
            {
                curState.Update();
            }
        }

        public IState Current()
        {
            return curState;
        }

        public void Set(string name)
        {
            IState state = null;

            if (states.ContainsKey(name))
            {
                state = states[name];
            }

            if (curState == state)
            {
                return;
            }

            if (null != prevState)
            {
                prevState.Exit();
            }

            prevState = curState;
            curState = state;

            if (null != curState)
                curState.Enter();
        }
    }
}
