using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class Transition : MonoBehaviour
    {
        public static Transition Instance { get; private set; }
        
        public float duration = 5.0f;
        [SerializeField] 
        private RectTransform overlayTransform;
        private Vector2 _maxSize;
        private RectTransform _rectTransform;

        [Header("Cool Effect (c)")]
        // TODO: if this is used in a project, don't cross-reference overlays, do everything from 1 scipt instead
        [SerializeField]
        private Transition otherOverlay;
        public bool canTransition = true;

        void Awake()
        {
            if (Instance != null)
            {
                // Destroy(this);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            var size = Mathf.Max(Screen.width, Screen.height);
            _rectTransform = GetComponent<RectTransform>();
            _maxSize = new Vector2(size, size) * 1.5f;
            _rectTransform.sizeDelta = Vector2.zero;
            
            // overlayTransform.sizeDelta = new Vector2(size, size) * 4;

            // Fade('i');
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U) && canTransition)
            {
                Fade('i');
            }
        }

        public void Fade(char mode, string sceneToLoad="Level 1")
        {
            // i: into the level
            // o: out of the level
            // r: restart the level
            // e: end the game
            
            // TODO: never do this shit again, actually
            
            // Recenter();
            if (mode is not 'i')
            {
                if (mode is not 'r')
                {
                    SoundEffectsPlayer.Instance.musicSourceAnimator.SetTrigger("Transition");
                    // un-DontDestroyOnLoad the SFX player:
                    SceneManager.MoveGameObjectToScene(SoundEffectsPlayer.Instance.transform.parent.gameObject,
                        SceneManager.GetActiveScene());
                }
                
                PlayerController.Instance.canMove = false;
                PlayerController.Instance.spriteAnimator.SetBool("IsWalking", false);
                PlayerController.Instance._rigidbody.velocity = Vector2.zero;
            }
            var img = GetComponent<Image>();
            img.raycastTarget = false;
            
            var end = mode == 'i' ? _maxSize : Vector2.zero;
            Debug.Log($"From {_rectTransform.rect.size} to {end}");

            canTransition = false;
            DOTween.To(() => _rectTransform.sizeDelta,
                x => _rectTransform.sizeDelta = x, end, duration).OnComplete(EnableEffect);
            
            img.raycastTarget = mode == 'i';
            PlayerController.Instance.canMove = true;

            if (mode is 'o' or 'r')
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }

        private void EnableEffect()
        {
            otherOverlay.canTransition = true;
            transform.SetSiblingIndex(0);
            _rectTransform.sizeDelta = Vector2.zero;
        }
        
        private void Recenter()
        {
            var centerBasedViewPortPosition = PlayerController.Instance.GetCenterBasedViewPortPosition();
            var canvasRect = transform.parent.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;

            _rectTransform.anchoredPosition = Vector3.Scale(centerBasedViewPortPosition, scale);
        }
    }
}
