using Deepwell;
using UnityEngine;

[RequireComponent(typeof(MoveRigidbody))]
public class FlipMoveRigidbodyOnCollision : MonoBehaviour
{
    [SerializeField] private DWTagValue elementTags = 0;

    private MoveRigidbody _moveRigidbody;

    private void Awake()
    {
        _moveRigidbody = GetComponent<MoveRigidbody>();
    }

    private void OnCollisionEnter2D(Collision2D collidedWith)
    {
        if (collidedWith.gameObject.MatchesTagMask<ElementTags>(elementTags))
        {
            _moveRigidbody.Flip();
        }
    }
}
