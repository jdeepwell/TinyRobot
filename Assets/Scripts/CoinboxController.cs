using System;
using System.Collections;
using System.Collections.Generic;
using Deepwell;
using UnityEngine;

public class CoinboxController : OnCollisionEvent
{
    [SerializeField] private uint coins = 10;

    private Animator _animator;

    private new void Awake()
    {
        _animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void ActOnCollisionEvent(CollisionEventInfo info)
    {
        if (coins > 0)
        {
            if (info.Intruder.MatchesTagMask<ElementTags>(ElementTags.IsPlayer) && info.Type.MatchesMask(CollisionEventType.HeadBumped)) dispenseCoin();
            else if (info.Intruder.MatchesTagMask<ElementTags>(ElementTags.IsCharacter)) dispenseCoin();
        }
    }

    private void dispenseCoin()
    {
        coins--;
        StaticGameManager.Instance.coins.value++;
        _animator.SetTrigger("FlipTheCoin");
        if (coins == 0)
        {
            var children = gameObject.GetComponentsInChildren<Transform>(true);
            children[1].gameObject.SetActive(false);
            children[2].gameObject.SetActive(true);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
    }
}
