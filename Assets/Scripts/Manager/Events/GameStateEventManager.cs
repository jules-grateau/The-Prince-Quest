using System;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class GameStateEventManager : MonoBehaviour
    {

        public static GameStateEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<Vector2, int> onAddScore;
        public void AddScore(Vector2 position, int score)
        {
            if (onAddScore != null)
            {
                onAddScore(position, score);
            }
        }

        public event Action<int> onUpdateScore;
        public void UpdateScore(int score)
        {
            if (onUpdateScore != null)
            {
                onUpdateScore(score);
            }
        }

        public event Action onStartGame;
        public void StartGame()
        {
            if (onStartGame != null)
            {
                onStartGame();
            }
        }

        public event Action onPauseGame;
        public void PauseGame()
        {
            if (onPauseGame != null)
            {
                onPauseGame();
            }
        }

        public event Action onResumeGame;
        public void ResumeGame()
        {
            if (onResumeGame != null)
            {
                onResumeGame();
            }
        }
    }
}