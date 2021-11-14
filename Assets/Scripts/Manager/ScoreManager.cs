using UnityEngine;

public class ScoreManager : MonoBehaviour
{
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
        eventManager.UpdateScore(this.score + this.levelScore);
    }
    
    void HandleLoadLevel(Level level)
    {
        this.score += this.levelScore;
        this.levelScore = 0;
        eventManager.UpdateScore(this.score + this.levelScore);
    }

    void HandleReloadLevel()
    {
        this.levelScore = 0;
        eventManager.UpdateScore(this.score + this.levelScore);
    }
} 