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
        [SerializeField] private AudioClip asteroidExplosion;
        [SerializeField] private AudioClip stickBendingPole;
        [SerializeField] private AudioClip releaseBendingPole;
        [SerializeField] private AudioClip metallicAsteroidHit;

        private void Awake()
        {
            Instance = this;
            _soundPlayer = this.GetComponent<AudioSource>();
            if (asteroidExplosion == null)
                asteroidExplosion = Resources.Load<AudioClip>("Sounds/AsteroidExplosion_SFX");
            if (metallicAsteroidHit == null)
                metallicAsteroidHit = Resources.Load<AudioClip>("Sounds/MetallicAsteroidHit_SFX");
            if (stickBendingPole == null)
                stickBendingPole = Resources.Load<AudioClip>("Sounds/StickBendingPole_SFX");
            if (releaseBendingPole == null)
                releaseBendingPole = Resources.Load<AudioClip>("Sounds/ReleaseBendingPole_SFX");
        }

        public void PlaySound(SoundName soundName)
        {
            switch (soundName)
            {
                case SoundName.AsteroidExplosion:
                    _soundPlayer.PlayOneShot(asteroidExplosion);
                    break;
                case SoundName.MetallicAsteroidHit:
                    _soundPlayer.PlayOneShot(metallicAsteroidHit);
                    break;
                case SoundName.StickBendingPole:
                    _soundPlayer.PlayOneShot(stickBendingPole);
                    break;
                case SoundName.ReleaseBendingPole:
                    _soundPlayer.PlayOneShot(releaseBendingPole);
                    break;
                default:
                    Debug.LogWarning("THERE'S NO SOUND LIKE THIS");
                    break;
            }
        }
    }
}
