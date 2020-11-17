using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Blastproof.Systems.Audio
{
    [Serializable]
    public class ClipData
    {
        public AudioClip clip;
        public bool hasAudioSource;
        public AudioSource audioSource;
        [MinMaxSlider(0, 1, true), SerializeField] private Vector2 VolumeRange;

        public float Volume => Random.Range(VolumeRange.x, VolumeRange.y);
    }

    [Serializable]
    public class Sounds
    {
        public ClipData[] clips;

    }


    [CreateAssetMenu(menuName = "Blastproof/Systems/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        [BoxGroup("SFX", true, true)] public Sounds HitObstacle;
        [BoxGroup("SFX", true, true)] public Sounds LowerRows;
        [BoxGroup("SFX", true, true)] public Sounds GameOver;
        [BoxGroup("SFX", true, true)] public Sounds NewBall;
        [BoxGroup("SFX", true, true)] public Sounds NewCoin;
        [BoxGroup("SFX", true, true)] public Sounds ButtonClick;
    }
}