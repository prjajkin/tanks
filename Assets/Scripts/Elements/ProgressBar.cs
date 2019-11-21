using UnityEngine;

namespace Assets.Scripts.Elements
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform filler;

        private float pixelsPerPercent;
        void Awake()
        {
            pixelsPerPercent = background.rect.width / 100f;
        }

        public void SetValue(float percent)
        {
            filler.offsetMax = new Vector2(-Mathf.Clamp(100 - percent, 0,100) *pixelsPerPercent, filler.offsetMax.y);
            
        }
        
    }
}
