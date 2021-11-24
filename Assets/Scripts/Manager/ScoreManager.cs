using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class ScoreManager : MonoBehaviour
    {
        public int scoreMaxLength = 7;

        private int _score = 0;
        private int _levelScore = 0;

        private GameStateEventManager _gameStateEventManager;
        private LevelEventManager _levelEventManager;

        private const string ScoreBoxPrefabPath = "Prefabs/Text/ScoreBox";
        private GameObject _scoreBoxPrefab;

        private void Start()
        {
            _gameStateEventManager = GameStateEventManager.current;
            _gameStateEventManager.onAddScore += HandleAddScore;
            _gameStateEventManager.onStartGame += HandleStartGame;

            _levelEventManager = LevelEventManager.current;
            _levelEventManager.onLoadLevel += HandleLoadLevel;
            _levelEventManager.onReloadLevel += HandleReloadLevel;

            _scoreBoxPrefab = Resources.Load<GameObject>(ScoreBoxPrefabPath);
        }

        private void OnDestroy()
        {
            _gameStateEventManager.onAddScore -= HandleAddScore;
            _gameStateEventManager.onStartGame -= HandleStartGame;

            _levelEventManager.onLoadLevel -= HandleLoadLevel;
            _levelEventManager.onReloadLevel -= HandleReloadLevel;
        }

        void HandleStartGame()
        {
            _score = 0;
            _levelScore = 0;
            UpdateScore();
        }

        void HandleAddScore(Vector2 position, int score)
        {
            InstantiateScoreBox(position, score);
            _levelScore += score;
            UpdateScore();
        }

        void InstantiateScoreBox(Vector2 position, int score)
        {
            TextMesh text = _scoreBoxPrefab.GetComponentInChildren<TextMesh>();
            if (text != null)
            {
                text.text = "";
                if (score >= 0)
                {
                    text.text = "+";
                }
                text.text += score.ToString();

            }

            Instantiate(_scoreBoxPrefab, new Vector3(position.x, position.y, _scoreBoxPrefab.transform.position.z), Quaternion.Euler(0, 0, 0));
        }

        void HandleLoadLevel(LevelType levelType)
        {
            _score += _levelScore;
            _levelScore = 0;
            UpdateScore();
        }

        void HandleReloadLevel()
        {
            _levelScore = 0;
            UpdateScore();
        }

        private void UpdateScore()
        {
            string newScore = (_score + _levelScore).ToString().PadLeft(scoreMaxLength, '0');
            UIEventManager.current.UpdateTextElement(UiTextElementType.Score, newScore);
        }
    }
}