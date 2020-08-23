using System;
using UnityEngine;

namespace Deepwell
{
    public class OneShotParticles : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        public event Action<GameObject> DeathObservers;
 
        private void Start() 
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
 
        private void Update()
        {
            if (!_particleSystem || _particleSystem.IsAlive()) return;
            DeathObservers?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}