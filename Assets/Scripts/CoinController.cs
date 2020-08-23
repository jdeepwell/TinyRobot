using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject coin;
    public GameObject collectionEffect;

    private Collider2D _collider;
    private ParticleSystem _collectionEffectParticleSystem;
    private Action _updateMode;

    private void Start()
    {
        _updateMode = Update_Idle;
        _collectionEffectParticleSystem = collectionEffect.GetComponent<ParticleSystem>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collidedWith)
    {
        if (collidedWith.gameObject.IsPlayer())
        {
            StaticGameManager.Instance.coins.value++;
            coin.SetActive(false);
            _collider.enabled = false;
            collectionEffect.SetActive(true);
            _collectionEffectParticleSystem.Play();
            _updateMode = Update_Collected;
        }
    }

    private void Update()
    {
        _updateMode.Invoke();
    }

    private void Update_Idle()
    {
        var rotY = Time.deltaTime * rotationSpeed;
        coin.transform.Rotate(new Vector3(0f, rotY, 0f));
    }
    
    private void Update_Collected()
    {
        var pos = collectionEffect.transform.position;
        pos.y += Time.deltaTime;
        collectionEffect.transform.position = pos;
        if (!_collectionEffectParticleSystem.isEmitting)
        {
            gameObject.SetActive(false);
            collectionEffect.SetActive(false);
            _updateMode = Update_Done;
        }
    }

    private void Update_Done()
    {
        // nothing to do...
    }
}
