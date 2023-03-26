using System;
using UnityEngine;

public class ThrowPointer : MonoBehaviour
{
    [SerializeField] 
    private float rotationSpeed = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        var playerScreenPos = PlayerController.Instance.GetScreenPosition();
        Debug.Log(playerScreenPos);
        var mousePosition = Input.mousePosition;
        var angle = Mathf.Atan2(mousePosition.y - playerScreenPos.y, 
                                    mousePosition.x - playerScreenPos.x) * Mathf.Rad2Deg;
        
        // i subtract 90 because the pointer in the prefab starts pointing to the sky
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 90.0f), 
                                             rotationSpeed * Time.deltaTime);
    }
}
