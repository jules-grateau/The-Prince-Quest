using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSoundController : MonoBehaviour
    {
        AudioSource audioSource;
        EventManager eventManager;
        AudioClip jumpAudioClip;
        private const string soundPath = "Sounds/";
        private const string jumpAudioClipPath = "PlayerJumpSound";
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            jumpAudioClip = Resources.Load<AudioClip>(soundPath + jumpAudioClipPath);
            eventManager = EventManager.current;
            eventManager.onPlayerJump += HandlePlayerJump;
        }

        private void OnDestroy()
        {
            eventManager.onPlayerJump -= HandlePlayerJump;
        }

        void HandlePlayerJump()
        {
            audioSource.PlayOneShot(jumpAudioClip);
        }

    }
}