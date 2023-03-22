using UnityEngine;

namespace Buttons
{
    public class LaserButton : Button
    {
        [SerializeField] 
        private Laser laser;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            if (CheckPress())
            {
                laser.lineRenderer.enabled = false;
                Destroy(laser);
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
        }
    }
}
