using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, ICanBeKilled
{
    [SerializeField] private GameObject respawnParticles = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip deathClip = null;
    [SerializeField] private AudioClip reSpawnClip = null;
    [SerializeField] private float wiggleAmount = 0.3f;

    private Action _updateMode;
    private PlayerMovement _playerMovement;
    private Collider2D _collider;
    private Vector3 _wiggleOrigin;
    private SpriteRenderer _spriteRenderer;

    // ICanBeKilled
    public void Kill()
    {
        StaticGameManager.Instance.KillPlayer();
    }
    
    private void ResetPlayer()
    {
        _playerMovement.StopPlayer();
        StartCoroutine(DeathAndReSpawn());
    }

    private IEnumerator DeathAndReSpawn()
    {
        DWInput.userInputEnabled = false;
        _collider.enabled = false;
        _wiggleOrigin = transform.position;
        _updateMode = Update_Wiggle;
        audioSource.InstantPlay(deathClip);
        yield return new WaitForSeconds(deathClip.length);
        _updateMode = Update_Respawning;
        _collider.enabled = true;
        var newPos = StaticGameManager.Instance.spawnPoint;
        transform.position = newPos;
        audioSource.InstantPlay(reSpawnClip);
        var newParticlesGO = Instantiate(respawnParticles, newPos, Quaternion.identity);
        var oneShotParticles = newParticlesGO.GetComponent<OneShotParticles>();
        var particlesDone = false;
        oneShotParticles.DeathObservers += (go) => particlesDone = true;
        yield return new WaitUntil(() => particlesDone == true);
        _SetPlayerAlpha(1f);
        _updateMode = Update_Idle;
        DWInput.userInputEnabled = true;
    }

    private void _SetPlayerAlpha(float alpha)
    {
        var c = _spriteRenderer.color;
        c.a = alpha;
        _spriteRenderer.color = c;
    }
    private void Update()
    {
        if (DWInput.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        _updateMode.Invoke();
    }

    private void Update_Idle()
    {
    }

    private void Update_Respawning()
    {
        _SetPlayerAlpha(Random.Range(0f, 1f));
    }

    private void Update_Wiggle()
    {
        var newPos = _wiggleOrigin + new Vector3(Random.Range(0f, wiggleAmount), Random.Range(0f, wiggleAmount), 0f);
        transform.position = newPos;
    }

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StaticGameManager.Instance.SceneResetEvent += ResetPlayer;
        _updateMode = Update_Idle;
        gameObject.AddTag<ElementTags>(ElementTags.IsPlayer);
    }

    private void OnDisable()
    {
        StaticGameManager.Instance.SceneResetEvent -= ResetPlayer;
    }
}

public static class PlayerController_GameObjectExtender
{
    public static bool IsPlayer(this GameObject go)
    {
        var playerMovementController = go.GetComponent<PlayerController>();
        return playerMovementController != null;
    }
}
