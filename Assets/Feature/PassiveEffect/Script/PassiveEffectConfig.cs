using UnityEngine;

namespace Feature.PassiveEffect.Script
{
    [CreateAssetMenu(fileName = "PassiveEffectConfig", menuName = "Configs/PassiveEffect")]
    public class PassiveEffectConfig : ScriptableObject
    {
        public Sprite Icon;

        [TextArea]
        public string Description;
    }
}