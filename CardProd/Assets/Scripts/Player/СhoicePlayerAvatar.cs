using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace Cards
{
    public class СhoicePlayerAvatar : MonoBehaviour
    {
        public Players m_players;
        [SerializeField] private GameObject m_avatarsPool;
        [SerializeField] private Transform m_avatarTransform;
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
        }

    }
}