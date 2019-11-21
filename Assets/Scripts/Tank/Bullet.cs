using Assets.Scripts.Components;
using Assets.Scripts.Enemies;
using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed; 
        [SerializeField] private float damage;
        [SerializeField] private string[] tagList; 
        [SerializeField] private LayerMask goalLayer; 

        public void SetVelocity(Vector3 direction)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.velocity = direction.normalized * speed;
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
            if ( Check(coll.gameObject))
            {
                var enemy = coll.gameObject.GetComponent<Enemy>();

                enemy?.GetDamage(damage);
                
            }
            Destroy(gameObject);
        }
        
    }
}
