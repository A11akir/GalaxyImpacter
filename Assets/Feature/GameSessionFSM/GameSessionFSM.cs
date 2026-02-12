using System;
using System.Collections.Generic;
using Feature.GameSessionData;
using UnityEngine;

namespace Feature.GameSessionFSM
{
    public class GameSessionFSM : MonoBehaviour
    {
        private StateGameSessionFSM StateCurrent { get; set; }
        private StartStateGameSessionFSM StartState { get; set; }
        private BanStateGameSessionFSM BanState { get; set; }
        private PickStateGameSessionFSM PickState { get; set; }
        private PrepareStateGameSessionFSM PrepareState { get; set; }
        private FightStateGameSessionFSM FightState { get; set; }
        private BlockStateGameSessionFSM BlockState { get; set; }
        
        private Dictionary<Type,StateGameSessionFSM> _states;

        private GameSessionFSM(StateGameSessionFSM stateCurrent)
        {
            StateCurrent = stateCurrent;
        }

        public void Initialize()
        {
            AddState(StartState);
            AddState(BanState);
            AddState(PickState);
            AddState(FightState);
            AddState(BlockState);
            
            SetState<StartStateGameSessionFSM>();
        }
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
    }
}