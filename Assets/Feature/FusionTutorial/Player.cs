using Fusion;
using UnityEngine;

namespace Feature.FusionTutorial
{
    public class Player : NetworkBehaviour
    {
        private const float MoveSpeed = 500f; // для UI обычно больше
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.Direction.Normalize();

                // движение в UI пространстве
                Vector2 move = new Vector2(data.Direction.x, data.Direction.z);

                _rect.anchoredPosition += move * MoveSpeed * Runner.DeltaTime;
            }
        }
    }
}
