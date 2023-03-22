using JetBrains.Annotations;
using UnityEngine;

namespace Buttons
{
    public class TrapdoorButton : Button
    {
        [SerializeField] 
        private Trapdoor trapdoor;

        [CanBeNull] 
        public Button buttonToDisable;

        public bool action = true;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (CheckPress())
            {
                trapdoor.Activate(action);
                trapdoor.isLocked = true;
                if (buttonToDisable != null)
                {
                    Destroy(buttonToDisable);
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (!PlayerPress && !CtrlPress)
            {
                buttonPressed = false;
            }
        }
    }
}
