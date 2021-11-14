using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public int scoreMaxLength = 5;
    private EventManager eventManager;
    private Text valueText;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = EventManager.current;
        eventManager.onUpdateScore += HandleUpdateScore;
        valueText = GetComponent<Text>();
    }

    void HandleUpdateScore(int score)
    {
        valueText.text = score.ToString().PadLeft(scoreMaxLength,'0');
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
