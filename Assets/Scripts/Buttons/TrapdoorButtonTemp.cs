using UnityEngine;

namespace Buttons
{
    public class TrapdoorButtonTemp : Button
    {
        [SerializeField] 
        private Trapdoor trapdoor;
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (trapdoor.isLocked) return;

            base.OnTriggerEnter2D(other);
            
            if (CheckPress())
            {
                trapdoor.Activate(true);
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (trapdoor.isLocked) return;

            base.OnTriggerExit2D(collision);
            
            if (!CheckPress())
            {
                trapdoor.Activate(false);
            }
        }
    }
}
