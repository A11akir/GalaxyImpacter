using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [CreateAssetMenu(fileName = "PassiveEffectConfig", menuName = "Configs/PassiveEffect")]
    public class PassiveEffectConfig : ScriptableObject
    {
        public Sprite Icon;

        [TextArea] [Tooltip("Используй {0} для подстановки текущего значения бонуса")]
        public string Description;
    }
}