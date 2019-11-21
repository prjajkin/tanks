using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] [Min(0.1f)] private float reloadTime=1;
        [SerializeField] private Transform bulletPoint;

        private bool canShot=true;
        private float shotTime;

        void CanShot()
        {
            if (canShot) return;
            shotTime += Time.deltaTime;

            if (shotTime > reloadTime)
            {
                shotTime = 0;
                canShot = true;
            }
        }

        public void TankShot()
        {
            if (!canShot){return;}

            canShot = false;
            var angle = Mathf.Atan2(bulletPoint.right.y, bulletPoint.right.x) * Mathf.Rad2Deg;
            var bullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.AngleAxis(angle, Vector3.forward)) ;
            bullet.SetVelocity(bulletPoint.forward);
        }

        private void Update()
        {
            CanShot();
        }
    }
}
