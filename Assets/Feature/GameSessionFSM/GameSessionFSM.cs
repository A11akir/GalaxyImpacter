using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Feature.GameSessionFSM
{
    public class GameSessionFSM : MonoBehaviour
    {
        private StateGameSessionFSM _stateCurrent;
        
        // Убираем приватные поля и инициализируем через DI
        private StartStateGameSessionFSM _startState;
        private BanStateGameSessionFSM _banState;
        private PickStateGameSessionFSM _pickState;
        private PrepareStateGameSessionFSM _prepareState;
        private FightStateGameSessionFSM _fightState;
        private BlockStateGameSessionFSM _blockState;
        
        private Dictionary<Type, StateGameSessionFSM> _states;
        
        // Добавляем Inject конструктор
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
            
            _states = new Dictionary<Type, StateGameSessionFSM>(); // ВАЖНО: инициализируем Dictionary!
        }

        public void Initialize()
        {
            // Добавляем все состояния
            AddState(_startState);
            AddState(_banState);
            AddState(_pickState);
            AddState(_prepareState);
            AddState(_fightState);
            AddState(_blockState);
            
            // Устанавливаем стартовое состояние
            SetState<StartStateGameSessionFSM>();
        }
        
        public void AddState(StateGameSessionFSM state)
        {
            if (state == null)
            {
                Debug.LogError($"Attempting to add null state to FSM");
                return;
            }
            
            var type = state.GetType();
            if (!_states.ContainsKey(type))
            {
                _states.Add(type, state);
                Debug.Log($"State added to FSM: {type.Name}");
            }
        }

        public void SetState<T>() where T : StateGameSessionFSM
        {
            var type = typeof(T);
            
            if (_stateCurrent != null && _stateCurrent.GetType() == type)
            {
                return;
            }

            if (_states.TryGetValue(type, out var newState))
            {
                _stateCurrent?.Exit();
                _stateCurrent = newState;
                _stateCurrent.Enter();
                
                Debug.Log($"FSM state changed to: {type.Name}");
            }
            else
            {
                Debug.LogError($"State {type.Name} not found in FSM dictionary!");
            }
        }
        
        public StateGameSessionFSM GetCurrentState()
        {
            return _stateCurrent;
        }
    }
}