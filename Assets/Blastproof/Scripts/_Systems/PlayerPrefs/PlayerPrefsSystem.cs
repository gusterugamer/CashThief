using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.PlayerPrefs
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/PlayerPrefs/PlayerPrefsSystem")]
    public class PlayerPrefsSystem : SerializedScriptableObject
    {
        [Button]
        public void DeleteAll()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            Debug.Log("Deleted all the player prefs");
        }
    }
}