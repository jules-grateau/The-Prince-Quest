using Assets.Scripts.Manager.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    [RequireComponent(typeof(Image))]
    public class HpDisplayController : MonoBehaviour
    {
        const string PathToSprite = "Sprites/UI/";
        const string SpriteFileName = "LifeSprite";

        Dictionary<int, Sprite> _healthToSprite;

        UIEventManager _uiEventManager;
        Image _image;
        // Use this for initialization
        void Start()
        {          
            _image = GetComponent<Image>();

            _healthToSprite = new Dictionary<int, Sprite>();

            Sprite[] lifeSprites = Resources.LoadAll<Sprite>(PathToSprite+ SpriteFileName);
            foreach(Sprite sprite in lifeSprites)
            {
                string numberString = sprite.name.Remove(0, SpriteFileName.Length+1);
                int spriteNumber;
                if (int.TryParse(numberString, out spriteNumber))
                {
                    _healthToSprite.Add(spriteNumber, sprite);
                }

            }
            _uiEventManager = UIEventManager.current;
            _uiEventManager.onUpdateLife += HandleUpdateLife;
        }

        void HandleUpdateLife(int life)
        {
            Sprite newSprite;
            if(_healthToSprite.TryGetValue(life,out newSprite))
            {
                _image.sprite = newSprite;
            }
        }
    }
}