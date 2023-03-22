using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    public bool isX;
    private float _xPosition;
    private float _yPosition;
    void Start()
    {
        _xPosition = transform.position.x;
        _yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isX)
        {
            transform.position = new Vector3(playerTransform.position.x, _yPosition, 0);
        }
        else
        {
            transform.position = new Vector3(_xPosition, playerTransform.position.y, 0);
        }

    }
}
