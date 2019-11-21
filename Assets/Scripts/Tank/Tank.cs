using Assets.Scripts.Components;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Tank
{
    public class Tank : MonoBehaviour
    {
        [SerializeField][Min(0.1f)] private float moveSpeed;
        [SerializeField][Min(0.1f)] private float rotateSpeed;
        [SerializeField][Range(0,0.99f)] private float defense;
        [SerializeField] private List<Gun> guns;
        [SerializeField] private Animator animator;
        
        public ReactiveProperty<float> HealtPercentRX = new ReactiveProperty<float>();
        
        private Rigidbody body;
        private int selectedGunNumber;
        private Health health;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            selectedGunNumber = 0;
            health.LessAllHP.AsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(false);
                GameManager.Singleton.StartGame();
            }).AddTo(this);
        }

        void FixedUpdate()
        {
            var v = Input.GetAxis("Vertical");
            var h = Input.GetAxis("Horizontal");
            body.AddForce(transform.forward * moveSpeed * v, ForceMode.Impulse);
            body.AddTorque(transform.up * h * Mathf.Sign(v), ForceMode.Impulse);
        }
        
        void Update()
        {
            guns[selectedGunNumber].transform.rotation = GunRotation();

            //Выстрел.
            if (Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.X))
            {
                guns[selectedGunNumber].TankShot();
            }

            //Сменить оружие.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeGun(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeGun(true);
            }
        }

        Quaternion GunRotation()
        {
            var mouse = Input.mousePosition;
            mouse.z = Camera.main.transform.position.z;
            var direction = Camera.main.ScreenToWorldPoint(mouse) - transform.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.up);
        }

        private void ChangeGun(bool nextAnotherPrevious)
        {
            if (guns.Count <= 1) {return; }

            guns[selectedGunNumber].gameObject.SetActive(false);
            if (nextAnotherPrevious)
            {
                selectedGunNumber = selectedGunNumber < guns.Count - 1 ? selectedGunNumber + 1 : 0;
            }
            else
            {
                selectedGunNumber = selectedGunNumber > 0 ? selectedGunNumber - 1 : guns.Count-1;
            }
            guns[selectedGunNumber].gameObject.SetActive(true);
        }

        public void GetDamage(float damageValue)
        {
            animator.SetTrigger("DamageTrigger");
            health.ChangeHP(-damageValue * defense);
            HealtPercentRX.Value = health.PercentHP;
        }

        public void ResetState()
        {
            animator.Rebind();
            health.RestartHP();
            HealtPercentRX.Value = health.PercentHP;
        }
        
    }
}
