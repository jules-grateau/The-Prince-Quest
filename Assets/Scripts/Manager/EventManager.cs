using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Enum;
using System;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager current;

        private void Awake()
        {
            current = this;
        }

      
    }
}