using System;
using System.Collections.Generic;

namespace Feature.GameSessionFSM
{
    public class GameSessionFSM
    {
        private StateGameSessionFSM StateCurrent { get; set; }
        
        private Dictionary<Type,StateGameSessionFSM> _states;

        public void AddState(StateGameSessionFSM state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : StateGameSessionFSM
        {
            var type = typeof(T);

            if (StateCurrent.GetType() == type)
            {
                return;
            }

            if (_states.TryGetValue(type, out var newState))
            {
                StateCurrent?.Exit();
                
                StateCurrent = newState;
                
                StateCurrent.Enter();
            }
        }

        public void Update()
        {
            StateCurrent?.Update();
        }
    }
    
    
    public class StateGameSessionFSM
    {
        protected readonly GameSessionFSM _gameSessionFSM;

        public StateGameSessionFSM(GameSessionFSM gameSessionFsm)
        {
            _gameSessionFSM = gameSessionFsm;
        }

        public virtual void Enter()
        {
            
        }
        public virtual void Exit()
        {
            
        }
        public virtual void Update()
        {
            
        }
        
    }
}