using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Cards
{
    public class AnimationComponent : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        private static readonly int ToHand = Animator.StringToHash("ToHand");
        private static readonly int ShakeCard = Animator.StringToHash("ShakeCard");
        private static readonly int ScaleCard = Animator.StringToHash("ScaleCard");
        private PlayerHand m_playerHand;
        public void Link(PlayerHand playerHand)
        {
            m_playerHand = playerHand;
        }

        public void AnimationFlipCard()
        {
            m_animator.SetTrigger(ToHand);
        }
        public void AnimationShakeCard()
        {
            m_animator.SetTrigger(ShakeCard);
        }

        public void AnimationScaleCard()
        { m_animator.SetTrigger(ScaleCard);
        }
        
        public void Flip()
        {
            Card card = GetComponent<Card>();
            card.StartCoroutine(card.MoveInHandOrTable(card, card.m_curParent, CardState.InHand));
        }
    }
}
