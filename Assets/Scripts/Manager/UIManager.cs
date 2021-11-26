using Assets.Scripts.Enum;
using Assets.Scripts.Manager.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Manager
{
    public class UIManager : MonoBehaviour
    {
        const string AddLifePrefabPath = "Prefabs/Text/LifeBox";
        const string ScorePrefabPath = "Prefabs/Text/ScoreBox";
        const string AddHealthPrefabPath = "Prefabs/Text/HealthPointBox";
        Dictionary<FloatingTextType, string> _floatingTextPaths = new Dictionary<FloatingTextType, string>
        {
            {FloatingTextType.AddLife, AddLifePrefabPath },
            {FloatingTextType.AddScore, ScorePrefabPath },
            {FloatingTextType.AddHealthPoint, AddHealthPrefabPath}
        };
        Dictionary<FloatingTextType, GameObject> _floatingTextPrefabs;
        UIEventManager _uiEventManager;
        // Use this for initialization
        void Start()
        {
            _floatingTextPrefabs = new Dictionary<FloatingTextType, GameObject>();
            foreach(KeyValuePair<FloatingTextType,string> pair in _floatingTextPaths)
            {
                _floatingTextPrefabs.Add(pair.Key, Resources.Load<GameObject>(pair.Value));
            }

            _uiEventManager = UIEventManager.current;
            _uiEventManager.onDisplayFloatingText += HandleDisplayFloatingText;
        }

        private void OnDestroy()
        {
            _uiEventManager.onDisplayFloatingText -= HandleDisplayFloatingText;
        }

        void HandleDisplayFloatingText(FloatingTextType type, string text, Vector2 position)
        {
            GameObject prefab;
            if(_floatingTextPrefabs.TryGetValue(type, out prefab))
            {
                TextMesh prefabText = prefab.GetComponentInChildren<TextMesh>();
                if(prefabText != null && !string.IsNullOrEmpty(text))
                {
                    prefabText.text = text;
                }

                Instantiate(prefab
                    , new Vector3(position.x, position.y, prefab.transform.position.z)
                    , Quaternion.Euler(0,0,0));
            }
        }
    }
}