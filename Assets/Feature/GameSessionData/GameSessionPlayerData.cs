using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feature.GameSessionData
{
    public class GameSessionPlayerData 
    {
        private List<GameplayCard> _cards = new List<GameplayCard>();

        public bool IsPlayerFirst;

        public string _heroName;

        public int _heroPowerData;
        public int _health;

        public bool PlayerHasHero()
        {
            if (_heroName != null && _heroPowerData != null && _health != null)
            {
                return true;
            }
            return false;
        }
    }

    public class Health
    {
        public event Action HpChanged;
        public event Action HealthOver;

        private float _currentHp;
        public float CurrentHp
        {
            get => _currentHp;
            private set
            {
                if (!Mathf.Approximately(_currentHp, value))
                {
                    _currentHp = value;
                    HpChanged?.Invoke();
                }
            }
        }

        public float MaxHp { get; set; }
        public void ResetHp() => CurrentHp = MaxHp;
        public void Heal(float amount)
        {
            
        }

        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;

            if (CurrentHp <= 0)
            {
                HealthOver?.Invoke();
            }
        }
    }

    public class HeroPowerData
    {
        private int cost;
        
    }

    public class GameplayCard
    {
        
    }

    public interface ITargetable
    {
        
    }
    public interface IUnTargetable
    {
        
    }

    public interface IMinion
    {
        List<GameplayCard> _cards { get; set; }
    }
}