using System;
using Deepwell;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private GameObject leftTile = null;
    [SerializeField] private GameObject rightTile = null;
    [SerializeField] private float sidesInsetPercent = 0f;

    private void Awake()
    {
        var tileSpriteRender = leftTile.GetComponent<SpriteRenderer>();
        var tileWidth = tileSpriteRender.sprite.bounds.size.x;
        var mySprite = GetComponent<SpriteRenderer>();
        var totalWidth = mySprite.size.x;
        var innerWidth = totalWidth - tileWidth * 2;
        var myCollider = GetComponent<BoxCollider2D>();
        mySprite.size = new Vector2(innerWidth, mySprite.size.y);
        myCollider.size = new Vector2(totalWidth - (sidesInsetPercent/100f)*tileWidth, myCollider.size.y);

        var shift = Vector3.zero;
        shift.x = innerWidth / 2f + (tileWidth / 2f);
        leftTile.transform.Translate(-shift);
        rightTile.transform.Translate(shift);
    }
}
