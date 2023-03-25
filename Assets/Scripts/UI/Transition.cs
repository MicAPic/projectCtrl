using System.Collections;
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

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            var size = Mathf.Max(Screen.width, Screen.height);
            _rectTransform = GetComponent<RectTransform>();
            _maxSize = new Vector2(size, size) * 3;
            _rectTransform.sizeDelta = Vector2.zero;
            
            overlayTransform.sizeDelta = new Vector2(size, size) * 4;

            StartCoroutine(Fade('i'));
        }

        public IEnumerator Fade(char mode, string sceneToLoad="Level 1")
        {
            Recenter();
            if (mode is 'o' or 'e')
            {
                SoundEffectsPlayer.GetInstance().musicSourceAnimator.SetTrigger("Transition");
                
                PlayerController.Instance.canMove = false;
                PlayerController.Instance.spriteAnimator.SetBool("IsWalking", false);
                PlayerController.Instance._rigidbody.velocity = Vector2.zero;
            }
            var img = GetComponent<Image>();
            img.raycastTarget = false;

            var currentTime = 0.0f;
            var start = _rectTransform.rect.size;
            var end = mode == 'i' ? _maxSize : Vector2.zero;
            Debug.Log($"From {start} to {end}");
            
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                _rectTransform.sizeDelta = Vector2.Lerp(start, end, currentTime / duration);
                yield return null;
            }

            _rectTransform.sizeDelta = end;
            img.raycastTarget = mode == 'i';
            PlayerController.Instance.canMove = true;

            if (mode == 'o')
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        
        private void Recenter()
        {
            var viewportPosition = Camera.main!.WorldToViewportPoint(PlayerController.Instance.transform.position);
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var canvasRect = transform.parent.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;

            _rectTransform.anchoredPosition = Vector3.Scale(centerBasedViewPortPosition, scale);
        }
    }
}
