using Assets.Scripts.Manager.Events;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundController : MonoBehaviour
    {
        AudioSource _audioSource;
        PlayerEventManager _playerEventManager;
        AudioClip _jumpAudioClip;
        
        public const string SoundPath = "Sounds/";
        public const string JumpAudioClipPath = "PlayerJumpSound";

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _jumpAudioClip = Resources.Load<AudioClip>(SoundPath + JumpAudioClipPath);
            _playerEventManager = PlayerEventManager.current;
            _playerEventManager.onPlayerJump += HandlePlayerJump;
        }

        private void OnDestroy()
        {
            _playerEventManager.onPlayerJump -= HandlePlayerJump;
        }

        void HandlePlayerJump()
        {
            _audioSource.PlayOneShot(_jumpAudioClip);
        }

    }
}