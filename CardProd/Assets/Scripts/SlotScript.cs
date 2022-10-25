using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{

    public class SlotScript : MonoBehaviour
    {
        public bool couple = false;
        
        public void SwitchCouple()
        {
            couple = !couple;
        }
    }
}