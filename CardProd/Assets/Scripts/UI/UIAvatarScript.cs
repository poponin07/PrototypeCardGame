using System;
using System.Collections;
using Cards;
using TMPro;
using UnityEngine;

public class UIAvatarScript : MonoBehaviour
{
    [SerializeField] private Transform[] Avatarcanvases;
    [SerializeField, Range(0.1f, 100f)] private float speedRotateCanvas;
    [SerializeField] public PlayerData _player1Data;
    [SerializeField] public PlayerData _player2Data;
    
    [SerializeField] private TextMeshProUGUI healthUItext_Player1;
    [SerializeField] private TextMeshProUGUI manaUItext_Player1;
    [SerializeField] private TextMeshProUGUI healthUItext_Player2;
    [SerializeField] private TextMeshProUGUI manaUItext_Player2;

    private void Start()
    {
        setDefaultData();
    }

    private void setDefaultData()
    {
        healthUItext_Player1.text = Convert.ToString(_player1Data.Health);
        manaUItext_Player1.text = Convert.ToString(_player1Data.Mana);
        healthUItext_Player2.text = Convert.ToString(_player2Data.Health);
        manaUItext_Player2.text = Convert.ToString(_player2Data.Mana);
    }

    public void RefreshPlayerManaRound(int manaPlayer1, int manaPlayer2)
    {
        manaUItext_Player1.text = manaPlayer1.ToString();
        manaUItext_Player2.text = manaPlayer2.ToString();
    }

    public void RefreshManaPlayer(int mana)
    {
        if (RoundManager.instance.PlayerMove == Players.Player1)
        {
            manaUItext_Player1.text = mana.ToString();
        }
        else
        {
            manaUItext_Player2.text = mana.ToString();
        }
    }
    public void RefreshHealthPlayer(int helth, bool isCardAttack)
    {
        if (isCardAttack)
        {

            if (RoundManager.instance.PlayerMove == Players.Player2)
            {
                healthUItext_Player1.text = helth.ToString();
            }
            else
            {
                healthUItext_Player2.text = helth.ToString();
            }
        }
        else
        {
            if (RoundManager.instance.PlayerMove == Players.Player1)
            {
                healthUItext_Player1.text = helth.ToString();
            }
            else
            {
                healthUItext_Player2.text = helth.ToString();
            }
        }
    }
    
    
    public IEnumerator CoroutineTurnIcon() //корутина поворота иконок hp и mana
    {
        if (Time.timeScale != 0)
        {
            float angle = 0;

            while (angle < 180)
            {
                float deltaAngle = speedRotateCanvas * Time.deltaTime;
                foreach (var icon in Avatarcanvases)
                {
                    icon.Rotate(Vector3.up, deltaAngle, Space.World);
                }
                angle += deltaAngle;
                yield return null;
            }
        }
    }
}
