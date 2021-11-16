using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Manager;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    #region Input Events

    public event Action<float> onHorizontalInput;
    public void HorizontalInput(float horizontalInput)
    {
        if(onHorizontalInput != null)
        {
            onHorizontalInput(horizontalInput);
        }
    }

    public event Action onSpaceInputDown;
    public void SpaceInputDown()
    {
        if(onSpaceInputDown != null)
        {
            onSpaceInputDown();
        }
    }

    public event Action onSpaceInput;
    public void SpaceInput()
    {
        if(onSpaceInput != null)
        {
            onSpaceInput();
        }
    }

    public event Action onEscapeInput;
    public void EscapeInput()
    {
        if(onEscapeInput!= null)
        {
            onEscapeInput();
        }
    }

    public event Action<bool> onStopPlayerInput;
    public void StopPlayerInput(bool shouldStop)
    {
        if (onStopPlayerInput != null)
        {
            onStopPlayerInput(shouldStop);
        }
    }
    #endregion

    #region State Event
    public event Action<bool> onWalking;
    public void Walking(bool isWalking)
    {
        if(onWalking != null)
        {
            onWalking(isWalking);
        }
    }

    public event Action<bool> onGrounded;
    public void Grounded(bool isGrounded)
    {
        if (onGrounded != null)
        {
            onGrounded(isGrounded);
        }
    }

    public event Action onPlayerDie;
    public void PlayerDie()
    {
        if (onPlayerDie != null)
        {
            onPlayerDie();
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

    public event Action<Vector2,int> onAddScore;
    public void AddScore(Vector2 position, int score)
    {
        if (onAddScore != null)
        {
            onAddScore(position,score);
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
        if(onStartGame != null)
        {
            onStartGame();
        }
    }

    public event Action onPauseGame;
    public void PauseGame()
    {
        if(onPauseGame != null)
        {
            onPauseGame();
        }
    }

    public event Action onResumeGame;
    public void ResumeGame()
    {
        if(onResumeGame != null)
        {
            onResumeGame();
        }
    }
    #endregion

    #region Collision events

    public event Action<int> onPlayerSteppedOnEnemy;
    public void PlayerSteppedOnEnemy(int enemyId)
    {
        if(onPlayerSteppedOnEnemy != null)
        {
            onPlayerSteppedOnEnemy(enemyId);
        }
    }

    public event Action<Vector2> onEnemyCollidedWithPlayer;
    public void EnemyCollidedWithPlayer(Vector2 direction)
    {
        if(onEnemyCollidedWithPlayer != null)
        {
            onEnemyCollidedWithPlayer(direction);
        }
    }

    public event Action<LevelType> onDoorEnter;
    public void DoorEnter(LevelType levelType)
    {
        if(onDoorEnter != null)
        {
            onDoorEnter(levelType);
        }
    }
    #endregion

    #region UI Events
    public event Action<ScreenType> onCloseScreen;
    public void CloseScreen(ScreenType screenType)
    {
        if(onCloseScreen != null)
        {
            onCloseScreen(screenType);
        }
    }

    public event Action<ScreenType> onOpenScreen;
    public void OpenScreen(ScreenType screenType)
    {
        if(onOpenScreen != null )
        {
            onOpenScreen(screenType);
        }
    }

    public event Action<ButtonType> onClickButton;
    public void ClickButton(ButtonType buttonType)
    {
        if(onClickButton != null)
        {
            onClickButton(buttonType);
        }
    }

    public event Action<ButtonType> onActivateButton;
    public void ActivateButton(ButtonType buttonType)
    {
        if(onActivateButton != null)
        {
            onActivateButton(buttonType);
        }
    }

    public event Action<UiTextElementType, string> onUpdateTextElement;
    public void UpdateTextElement(UiTextElementType elementType, string value)
    {
        if(onUpdateTextElement != null)
        {
            onUpdateTextElement(elementType, value);
        }
    }

    #endregion

    #region Level Events

    public event Action<LevelType> onLoadLevel;
    public void LoadLevel(LevelType levelType)
    {
        if(onLoadLevel != null)
        {
            onLoadLevel(levelType);
        }
    }

    public event Action onReloadLevel;
    public void ReloadLevel()
    {
        if(onReloadLevel != null)
        {
            onReloadLevel();
        }
    }
    #endregion

    #region Animation Events 
    public event Action<bool> onIsKissing;
    public void IsKissing(bool isKissing)
    {
        if(onIsKissing != null)
        {
            onIsKissing(isKissing);
        }
    }

    public event Action onStopKissing;
    public void StopKissing()
    {
        if (onStopKissing != null)
        {
            onStopKissing();
        }
    }

    public event Action<AnimationType> onStartAnimation;
    public void StartAnimation(AnimationType animationType)
    {
        if(onStartAnimation != null)
        {
            onStartAnimation(animationType);
        }
    }

    public event Action<AnimationType> onStopAnimation;
    public void StopAnimation(AnimationType animationType)
    {
        if (onStopAnimation != null)
        {
            onStopAnimation(animationType);
        }
    }

    public event Action<AnimationType> onTriggerAnimation;
    public void TriggerAnimation(AnimationType animationType)
    {
        if(onTriggerAnimation != null)
        {
            onTriggerAnimation(animationType);
        }
    }
    #endregion

    #region Player Event
    public event Action onPlayerJump;
    public void PlayerJump()
    {
        if(onPlayerJump != null)
        {
            onPlayerJump();
        }
    }
    #endregion
}
