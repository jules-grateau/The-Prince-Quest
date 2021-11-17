using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int scoreMaxLength = 7;
    private int score = 0;
    private int levelScore = 0;
    private EventManager eventManager;
    private const string ScoreBoxPrefabPath = "Prefabs/Text/ScoreBox";
    private GameObject scoreBoxPrefab;
    private void Start()
    {
        eventManager = EventManager.current;
        eventManager.onAddScore += HandleAddScore;
        eventManager.onLoadLevel += HandleLoadLevel;
        eventManager.onReloadLevel += HandleReloadLevel;
        eventManager.onStartGame += HandleStartGame;
        scoreBoxPrefab = Resources.Load<GameObject>(ScoreBoxPrefabPath);
    }

    void HandleStartGame()
    {
        score = 0;
        levelScore = 0;
        UpdateScore();
    }

    void HandleAddScore(Vector2 position, int score)
    {
        InstantiateScoreBox(position, score);
        this.levelScore += score;
        UpdateScore();
    }

    void InstantiateScoreBox(Vector2 position, int score)
    {
        TextMesh text = scoreBoxPrefab.GetComponentInChildren<TextMesh>();
        if(text != null)
        {
            text.text = "";
            if(score >= 0)
            {
                text.text = "+";
            }
            text.text += score.ToString();

        }

        Instantiate(scoreBoxPrefab, new Vector3(position.x, position.y,scoreBoxPrefab.transform.position.z), Quaternion.Euler(0,0,0));
    }
    
    void HandleLoadLevel(LevelType levelType)
    {
        this.score += this.levelScore;
        this.levelScore = 0;
        UpdateScore();
    }

    void HandleReloadLevel()
    {
        this.levelScore = 0;
        UpdateScore();
    }

    private void UpdateScore()
    {
        string newScore = (this.score + this.levelScore).ToString().PadLeft(scoreMaxLength, '0');
        eventManager.UpdateTextElement(UiTextElementType.Score, newScore);
    }
} 