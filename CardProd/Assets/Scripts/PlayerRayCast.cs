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
    public class PlayerRayCast : MonoBehaviour
    {
        private InputSystem_i _input;
        private Ray ray; 
        [SerializeField] private CardManager _cardManager;

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
                if (card == null) return;
                           
                    if (card.m_cardState == CardState.InDeck)
                {
                    card.m_cardState = CardState.Discard;
                    var cardComp = hit.transform != null ? card : null;
                if (cardComp != null)
                {
                    _cardManager.GetCardFromDeck();
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