using System;
using UnityEngine;

namespace Controllers
{
    public class SoundController : MonoBehaviour
    {
        public enum SoundName
        {
            AsteroidExplosion,
            MetallicAsteroidHit,
            StickBendingPole,
            ReleaseBendingPole
        }
        
        public static SoundController Instance { get; private set; }

        private AudioSource _soundPlayer;
        [SerializeField] private AudioClip _asteroidExplosionSound;
        [SerializeField] private AudioClip _stickBendingPoleSound;
        [SerializeField] private AudioClip _releaseBendingPoleSound;
        [SerializeField] private AudioClip _metallicAsteroidHitSound;

        private void Awake()
        {
            Instance = this;
            _soundPlayer = this.GetComponent<AudioSource>();
            if (_asteroidExplosionSound == null)
                _asteroidExplosionSound = Resources.Load<AudioClip>("AsteroidExplosion_SFX");
            if (_metallicAsteroidHitSound == null)
                _metallicAsteroidHitSound = Resources.Load<AudioClip>("MetallicAsteroidHit_SFX");
            if (_stickBendingPoleSound == null)
                _stickBendingPoleSound = Resources.Load<AudioClip>("StickBendingPoleSound_SFX");
            if (_releaseBendingPoleSound == null)
                _releaseBendingPoleSound = Resources.Load<AudioClip>("ReleaseBendingPoleSound_SFX");
        }

        public void PlaySound(SoundName soundName)
        {
            switch (soundName)
            {
                case SoundName.AsteroidExplosion:
                    _soundPlayer.PlayOneShot(_asteroidExplosionSound);
                    break;
                case SoundName.MetallicAsteroidHit:
                    _soundPlayer.PlayOneShot(_metallicAsteroidHitSound);
                    break;
                case SoundName.StickBendingPole:
                    _soundPlayer.PlayOneShot(_stickBendingPoleSound);
                    break;
                case SoundName.ReleaseBendingPole:
                    _soundPlayer.PlayOneShot(_releaseBendingPoleSound);
                    break;
                default:
                    Debug.Log("THERE'S NO SOUND LIKE THIS");
                    break;
            }
        }
    }
}
