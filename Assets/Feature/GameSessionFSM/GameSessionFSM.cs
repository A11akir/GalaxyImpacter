using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Feature.GameSessionFSM
{
    public class GameSessionFSM : MonoBehaviour
    {
        private StateGameSessionFSM _stateCurrent;
        
        private StartStateGameSessionFSM _startState;
        private BanStateGameSessionFSM _banState;
        private PickStateGameSessionFSM _pickState;
        private PrepareStateGameSessionFSM _prepareState;
        private FightStateGameSessionFSM _fightState;
        private BlockStateGameSessionFSM _blockState;
        
        private Dictionary<Type, StateGameSessionFSM> _states;
        
        [Inject]
        public void Construct(
            StartStateGameSessionFSM startState,
            BanStateGameSessionFSM banState,
            PickStateGameSessionFSM pickState,
            PrepareStateGameSessionFSM prepareState,
            FightStateGameSessionFSM fightState,
            BlockStateGameSessionFSM blockState)
        {
            _startState = startState;
            _banState = banState;
            _pickState = pickState;
            _prepareState = prepareState;
            _fightState = fightState;
            _blockState = blockState;
            
            _states = new Dictionary<Type, StateGameSessionFSM>();
        }

        public void Initialize()
        {
            AddState(_startState);
            AddState(_banState);
            AddState(_pickState);
            AddState(_prepareState);
            AddState(_fightState);
            AddState(_blockState);
            
            SetState<StartStateGameSessionFSM>();
        }
        
        public void AddState(StateGameSessionFSM state)
        {
            if (state == null) return;
            
            var type = state.GetType();
            if (!_states.ContainsKey(type))
            {
                _states.Add(type, state);
            }
        }

        public void SetState<T>() where T : StateGameSessionFSM
        {
            var type = typeof(T);
            
            if (_stateCurrent != null && _stateCurrent.GetType() == type)
                return;

            if (_states.TryGetValue(type, out var newState))
            {
                _stateCurrent?.Exit();
                _stateCurrent = newState;
                _stateCurrent.Enter();
            }
        }
        
        public StateGameSessionFSM GetCurrentState() => _stateCurrent;
    }
}