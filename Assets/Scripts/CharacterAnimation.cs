using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    // [SerializeField] private IMovementSpeedProvider movementSpeedProvider = null;
    private Animator _animator;
    private int _speedId;
    private IMovementSpeedProvider _movementSpeedProvider;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _speedId = Animator.StringToHash("Speed");
        _movementSpeedProvider = GetComponent<IMovementSpeedProvider>();
        Assert.IsNotNull(_movementSpeedProvider, "CharacterAnimation needs a component implementing IMovementSpeedProvider");
    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetFloat(_speedId, _movementSpeedProvider.NormalizedMovementSpeed);
    }
}

public interface IMovementSpeedProvider
{
    // should hold a value between 0.0 (standing still) and 1.0 (moving at maximum speed in any direction)
    float NormalizedMovementSpeed { get; }
}
