using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWithParticle : MonoBehaviour
{
    [SerializeField] 
    private float distanceRay = 100;
    [SerializeField] 
    private Transform laserFirePoint;
    public LineRenderer lineRenderer;

    //public ParticleSystem laserStartParticles;
    public ParticleSystem laserEndParticles;

    private bool endParticlesPlaying;
    //private bool startParticlesPlaying;

    private void Start()
    {
        lineRenderer.material.color = Color.white;
    }

    private void Update()
    {
        ShootLaser();
    }
    
    private bool soundWasPlayed;

    private void ShootLaser()
    {
        if (Physics2D.Raycast(transform.position, transform.right))
        {
            RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
            if (hit)
            {
                if (hit.collider is CapsuleCollider2D)
                {
                    if (!soundWasPlayed)
                    {
                        SoundEffectsPlayer.Instance.Hurt();
                        soundWasPlayed = true;
                    }
                    Debug.Log(hit.collider);
                    PlayerController.Instance.Restart();
                }

                if (!endParticlesPlaying)
                {
                    endParticlesPlaying = true;
                    laserEndParticles.Play(true);
                }
                laserEndParticles.gameObject.transform.position = hit.point;
                float distance = ((Vector2)hit.point - (Vector2)transform.position).magnitude;
                lineRenderer.SetPosition(1, new Vector3(distance, 0, 0));
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

    public void DisableParticle()
    {
        Destroy(laserEndParticles);
    }

    


}
