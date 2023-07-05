using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Player;
using UnityEngine;

namespace Cards
{
    public class СhoicePlayerAvatar : MonoBehaviour
    {
        public Players m_players;
        [SerializeField] private GameObject m_avatarsPool;
        [SerializeField] private Transform m_avatarTransform;
        [SerializeField] private PlayerScript m_playerScript;
        [SerializeField] private BaseAbilities m_ability;
        public bool isSetAvatar;

        private void Start()
        {
            isSetAvatar = false;
        }

        public void SetAvatar()
        {
            transform.position = m_avatarTransform.position;
            transform.SetParent(m_avatarTransform);
            isSetAvatar = true;
            m_avatarsPool.SetActive(false);
            m_playerScript.SetHeroParams(m_ability);
        }

    }
}