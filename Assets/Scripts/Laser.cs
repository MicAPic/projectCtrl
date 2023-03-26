using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float distanceRay = 100;
    [SerializeField] private Transform laserFirePoint;
    public LineRenderer lineRenderer;

    // private void Awake()
    // {
    //     
    // }


    private void Update()
    {
        ShootLaser();
    }

    private bool _isSoundPlayed;
    private void ShootLaser()
    {
        if (Physics2D.Raycast(transform.position, transform.right))
        {
            RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
            if (hit.collider is CapsuleCollider2D)
            {
                Debug.Log(hit.collider);
                if (!_isSoundPlayed)
                {
                    SoundEffectsPlayer.GetInstance().Hurt();
                    _isSoundPlayed = true;
                }
                PlayerController.Instance.Restart();
            }
            Draw2DRay(laserFirePoint.position, hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * distanceRay); 
        }
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
