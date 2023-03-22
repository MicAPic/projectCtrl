using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool _isEndPosition;
    private Vector2 _startPosition;
    public Vector2 _endPosition = new Vector2(0f, 0f);
    //public float Y_openOffset;
    float _activeSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.localPosition;
        _isEndPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isEndPosition)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _endPosition, _activeSpeed * Time.deltaTime);
            if(Mathf.Abs(transform.localPosition.x - _endPosition.x) <= 0.1 && Mathf.Abs(transform.localPosition.y - _endPosition.y) <= 0.1)
            {
                Debug.Log("sefdsf");
                _isEndPosition = true;
            }
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _startPosition, _activeSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.localPosition.x - _startPosition.x) <= 0.1 && Mathf.Abs(transform.localPosition.y - _startPosition.y) <= 0.1)
            {
                _isEndPosition = false;
            }
        }
    }
}
