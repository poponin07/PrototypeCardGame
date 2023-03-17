using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static Cards.Card;

namespace Input
{
    public class PlayerRayCast : MonoBehaviour // дебажный скрипт для удобства
    {
        private InputSystem_i _input;
        private Ray ray;
        [SerializeField] private CardManager m_cardManager;

        private void Awake()
        {
            _input = new InputSystem_i();
        }

        private void Start()
        {
            _input.Camera.ClickForRayCast.canceled += context =>  OnRayCastPlayer();
        }

        private void OnRayCastPlayer()
        {
            Camera _camera = Camera.main;
            ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit))
            {
                Card card = hit.transform.GetComponent<Card>();
                Debug.Log(hit.collider.gameObject.name);
                if (card == null || card.players == RoundManager.instance.PlayerMove)
                {
                    СhoicePlayerAvatar сhoicePlayerAvatar = hit.transform.GetComponent<СhoicePlayerAvatar>();
                    
                    if (сhoicePlayerAvatar != null && сhoicePlayerAvatar.m_players ==  RoundManager.instance.PlayerMove)
                    {
                        сhoicePlayerAvatar.SetAvatar();
                        return;
                    }
                    return;
                }
                
                 
                    Card cardComp = hit.transform != null ? card : null;
                    if (cardComp != null && cardComp.m_cardState == CardState.InHand /*&& RoundManager.instance.GetSwapCardOnFerstRoundIndex() > 0 */)
                    {
                        Debug.Log("click");
                        if (cardComp.CardInDeck())
                        {
                            m_cardManager.AddCardToDeck(cardComp);
                            m_cardManager.GetCardFromDeck(1, true); 
                        }
                    }
            }
        }

        
        
        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDestroy()
        {
            _input.Disable();
        }
    }
}