using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    //[SerializeField] private Transform _playerTransform;
    [SerializeField] private EnemyController _controller;
    [SerializeField] private Transform flipCheckPoint;
    [SerializeField] private Transform platformTransform;
    //[SerializeField] private Animator _animator;

    public float _runSpeed = 30f;
    public float _horizontalMove = 0f;
    public float _rightBound = 5f;
    public float _leftBound = 5f;
    private float _startXPosition;


    void Start()
    {
        _startXPosition = transform.position.x;
        _horizontalMove = _runSpeed;
        _rightBound = platformTransform.position.x + platformTransform.localScale.x/2;
        _leftBound = platformTransform.position.x - platformTransform.localScale.x/2;
    }
    void Update()
    {
        switch (_controller._currentState)
        {
            case EnemyController.State.RUN:
                RunBehavior();
                break;
            case EnemyController.State.SCARED:
                ScaredBehavior();
                break;
        }
    }
    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime);
    }

    private void RunBehavior()
    {
        if ((flipCheckPoint.transform.position.x <= _leftBound) || (flipCheckPoint.transform.position.x >= _rightBound))
            _horizontalMove *= -1;
        //_animator.SetFloat("Speed", Math.Abs(_horizontalMove));
    }

    public bool changeStateFirstTime;
    private void ScaredBehavior()
    {
        Debug.Log("Scared3");
        if (changeStateFirstTime)
        {
            _horizontalMove = _horizontalMove * (-1) / 3;
            Debug.Log("Scared5");
            Debug.Log(_horizontalMove);
            changeStateFirstTime = false;
        }
        //_animator.SetFloat("Speed", Math.Abs(_horizontalMove));
    }
}
