using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class EnemyEventManager : MonoBehaviour
    {

        public static EnemyEventManager current;

        private void Awake()
        {
            current = this;
        }


        public event Action<Vector2> onEnemyCollidedWithPlayer;
        public void EnemyCollidedWithPlayer(Vector2 direction)
        {
            if (onEnemyCollidedWithPlayer != null)
            {
                onEnemyCollidedWithPlayer(direction);
            }
        }

        public event Action<int> onEnemyDie;
        public void EnemyDie(int enemyId)
        {
            if (onEnemyDie != null)
            {
                onEnemyDie(enemyId);
            }

        }
    }
}