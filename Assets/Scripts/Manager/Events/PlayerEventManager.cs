using Assets.Scripts.Controllers.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Manager.Events
{
    public class PlayerEventManager : MonoBehaviour
    {
        public static PlayerEventManager current;

        private void Awake()
        {
            current = this;
        }

        public event Action<bool> onWalking;
        public void Walking(bool isWalking)
        {
            if (onWalking != null)
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

        //Event sent by the PlayerStatusController indicating that the player just died
        public event Action onPlayerDie;
        public void PlayerDie()
        {
            if (onPlayerDie != null)
            {
                onPlayerDie();
            }
        }

        //Event sent by other element indicating they kill the played
        public event Action onKillPlayer;
        public void KillPlayer()
        {
            if (onKillPlayer != null)
            {
                onKillPlayer();
            }
        }

        public event Action<Vector2> onTakeBonus;
        public void TakeBonus(Vector2 position)
        {
            if (onTakeBonus != null)
            {
                onTakeBonus(position);
            }
        }

        public event Action onPlayerJump;
        public void PlayerJump()
        {
            if (onPlayerJump != null)
            {
                onPlayerJump();
            }
        }

        public event Action<int> onCanInteractWith;
        public void CanInteractWith(int gameobjectId)
        {
            if (onCanInteractWith != null)
            {
                onCanInteractWith(gameobjectId);
            }
        }

        public event Action<GameObject, int> onStartInteractWith;
        public void StartInteractWith(GameObject interactionFrom, int gameobjectId)
        {
            if (onStartInteractWith != null)
            {
                onStartInteractWith(interactionFrom, gameobjectId);
            }
        }

        public event Action<int> onStopInteractWith;
        public void StopInteractWith(int gameobjectId)
        {
            if (onStopInteractWith != null)
            {
                onStopInteractWith(gameobjectId);
            }
        }

        public event Action<int> onStartDragging;
        public void StartDragging(int gameobjectId)
        {
            if (onStartDragging != null)
            {
                onStartDragging(gameobjectId);
            }
        }

        public event Action<int> onStopDragging;
        public void StopDragging(int gameobjectId)
        {
            if (onStopDragging != null)
            {
                onStopDragging(gameobjectId);
            }
        }

        public event Action<int> onPlayerSteppedOnEnemy;
        public void PlayerSteppedOnEnemy(int enemyId)
        {
            if (onPlayerSteppedOnEnemy != null)
            {
                onPlayerSteppedOnEnemy(enemyId);
            }
        }

        public event Action<bool> onIsKissing;
        public void IsKissing(bool isKissing)
        {
            if (onIsKissing != null)
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

        public event Action<bool> onPlayerInvulnerability;
        public void PlayerInvulnerability(bool isInvulnerable)
        {
            if(onPlayerInvulnerability != null)
            {
                onPlayerInvulnerability(isInvulnerable);
            }
        }

        public event Action<PlayerStatus> onSetPlayerStatus;
        public void SetPlayerStatus(PlayerStatus playerStatus)
        {
            if(onSetPlayerStatus != null)
            {
                onSetPlayerStatus(playerStatus);
            }
        }

        public event Action<PlayerStatus> onUpdatePlayerStatus;
        public void UpdatePlayerStatus(PlayerStatus playerStatus)
        {
            if (onUpdatePlayerStatus != null)
            {
                onUpdatePlayerStatus(playerStatus);
            }
        }
    }
}