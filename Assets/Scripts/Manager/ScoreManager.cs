using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int scoreMaxLength = 7;
    private int score = 0;
    private int levelScore = 0;
    private EventManager eventManager;

    private void Start()
    {
        eventManager = EventManager.current;
        eventManager.onAddScore += HandleAddScore;
        eventManager.onLoadLevel += HandleLoadLevel;
        eventManager.onReloadLevel += HandleReloadLevel;
    }

    void HandleAddScore(int score)
    {
        this.levelScore += score;
        UpdateScore();
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