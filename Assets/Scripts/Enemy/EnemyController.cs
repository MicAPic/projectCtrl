using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _ctrlLayer;
    [SerializeField] private EnemyMovement enemyMovement;
    //[SerializeField] private Animator _animator;
    //[SerializeField] private Transform _playerTransform;


    [Range(0, 0.3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    [SerializeField] private BoxCollider2D boxCollider;
    private float _rangeX = 20;
    private float _rangeY = 2.5f;
    private float _colliderDistanceX = 0.21f;
    private float _colliderDistanceY = 0.4f;

    public bool _lookAtRight = false;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;

    public enum State {RUN, SCARED};
    public State _currentState;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _currentState = State.RUN;
    }
    void Update()
    {
        switch (_currentState)
        {
            case State.RUN:
                if (CTRLInSight())
                {
                    _currentState = State.SCARED;
                    enemyMovement.changeStateFirstTime = true;
                    //_animator.SetBool("Scared", true);
                }
                break;
            case State.SCARED:
                Debug.Log("Scared2");
                if (!CTRLInSight())
                {
                    Debug.Log("Scared4");
                    if(enemyMovement._horizontalMove > 0)
                    {
                        enemyMovement._horizontalMove = enemyMovement._runSpeed;
                    }
                    else
                    {
                        enemyMovement._horizontalMove = (-1)*enemyMovement._runSpeed;
                    }
                    _currentState = State.RUN;
                    //Flip();
                }
                break;
        }
    }
    public void Move(float move)
    {
        if (_currentState == State.RUN){
            Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
            ChangeDirection(move);
        }
        else if (_currentState == State.SCARED)
        {
            Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
            _rigidbody.velocity = Vector2.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        }
        //ChangeDirection(move);
    }


    public void Flip()
    {
        _lookAtRight = !_lookAtRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void ChangeDirection(float move)
    {
        if (move > 0 && !_lookAtRight)
        {
            Flip();
        }
        else if (move < 0 && _lookAtRight)
        {
            Flip();
        }
    }

    private bool CTRLInSight()
    {
        RaycastHit2D hit =
                    Physics2D.BoxCast(boxCollider.bounds.center - transform.right * _rangeX * transform.localScale.x * _colliderDistanceX + transform.up * _rangeY * _colliderDistanceY,
                    new Vector3(boxCollider.bounds.size.x * _rangeX, boxCollider.bounds.size.y * _rangeY, boxCollider.bounds.size.z),
                    0, Vector2.left, 0, _ctrlLayer);
        if (hit.collider == null) return false;
        if (hit.transform.CompareTag("Player"))
        {
            if (!ControlController.Instance.isHeld) return false;
        }
        return true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center - transform.right * _rangeX * transform.localScale.x * _colliderDistanceX + transform.up * _rangeY * _colliderDistanceY,
            new Vector3(boxCollider.bounds.size.x * _rangeX, boxCollider.bounds.size.y * _rangeY, boxCollider.bounds.size.z));
    }
}
