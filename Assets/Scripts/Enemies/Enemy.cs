using Assets.Scripts.Components;
using Assets.Scripts.Tank;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] [Min(0.1f)] private float speed;
        [SerializeField] [Min(0.1f)] private float damage;
        [SerializeField] [Range(0, 0.99f)] private float defense;
        [SerializeField] private LayerMask goalLayer;

        public UnityEvent Destroyed;

        private Rigidbody body;
        private Transform goalTransform;
        private Health health;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            health.LessAllHP.AddListener(Destroyed.Invoke);
        }

        public void SetGoal(Transform goalTransform)
        {
            this.goalTransform = goalTransform;
        }

        void FixedUpdate()
        {
            if (goalTransform == null){return;}

            var direct = (goalTransform.position - transform.position).normalized;
            body.AddForce(direct * speed , ForceMode.Impulse);
        }

        private bool Check(GameObject obj)
        {
            if (((1 << obj.layer) & goalLayer.value) != 0)
            {
                return true;
            }
            return false;
        }

        private void OnCollisionEnter(Collision coll)
        {
            if (Check(coll.gameObject))
            {
                var tank = coll.gameObject.GetComponent<Tank.Tank>();
                tank?.GetDamage(damage);
                Destroyed.Invoke();
                Destroy(gameObject);
            }
            
        }

        public void GetDamage(float damageValue)
        {
            health.ChangeHP(-damageValue * defense);
        }

    }
}
