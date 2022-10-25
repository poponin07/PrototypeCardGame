using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvatarScript : MonoBehaviour
{
    [SerializeField] private Transform[] Avatarcanvases;
    [SerializeField, Range(0.1f, 100f)] private float speedRotateCanvas;

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
