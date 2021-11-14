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
        Debug.Log("Adding score to the level score");
        this.levelScore += score;
        eventManager.UpdateScore(this.score + this.levelScore);
    }
    
    void HandleLoadLevel(Level level)
    {
        Debug.Log("Adding score to the total score");
        this.score += this.levelScore;
        this.levelScore = 0;
        eventManager.UpdateScore(this.score + this.levelScore);
    }

    void HandleReloadLevel()
    {
        Debug.Log("Reseting level score");
        this.levelScore = 0;
        eventManager.UpdateScore(this.score + this.levelScore);
    }
} 
