using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    [SerializeField] 
    private Transform playerTransform;
    [SerializeField]
    private bool isX;
    [SerializeField] 
    private float yOffset;
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
        transform.position = isX ? new Vector3(playerTransform.position.x, _yPosition, 0) : 
                                   new Vector3(_xPosition, playerTransform.position.y + yOffset, 0);
    }
}
