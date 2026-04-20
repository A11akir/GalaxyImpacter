using System;
using DG.Tweening;
using Feature.GameSessionData;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.Card.Script
{
    public class NonTargetHeroPowerCastBehaviour : MonoBehaviour,
        IPointerClickHandler,
        ITransformCastCardBehaviour
    {
        public bool _canCastCard { get; set; }
        private CardAndHealthEntityOwnerData _owner;

        public event Action<CardAndHealthEntityOwnerData, CardAndHealthEntityOwnerData> OnTryCardCast;

        public void SetOwner(CardAndHealthEntityOwnerData owner) => _owner = owner;
        public void CanCastCard(bool canCast) => _canCastCard = canCast;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canCastCard) return;
            PlayCastAnimation();
        }

        private void PlayCastAnimation()
        {
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOLocalMoveY(transform.localPosition.y + 30f, 0.1f).SetEase(Ease.OutQuad))
                .Join(transform.DOLocalRotate(new Vector3(0, 360f, 0), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear))
                .Append(transform.DOLocalMoveY(transform.localPosition.y, 0.1f).SetEase(Ease.InQuad))
                .OnComplete(() => TryCastCard(this));
        }

        public void TryCastCard(ITransformCastCardBehaviour currentCardBehaviour)
        {
            OnTryCardCast?.Invoke(_owner, null);
        }   
    }
}