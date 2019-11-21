using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float hP = 100;
        [SerializeField] private bool autoDestroy;

        [HideInInspector]public float CurrentHP;
        public float PercentHP => CurrentHP / hP*100;
        public UnityEvent LessAllHP;
        
        void Awake()
        {
            RestartHP();
        }

        public virtual void ChangeHP(float value)
        {
            CurrentHP += value;

            if (CurrentHP <= 0)
            {
                LessAllHP?.Invoke();
                if (autoDestroy)
                {
                    Destroy();
                }
            }
        }

        public void RestartHP()
        {
            CurrentHP = hP;
        }
        protected virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
