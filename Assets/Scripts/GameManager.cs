using Assets.Scripts.Enemies;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Elements;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private Tank.Tank tankPrefab;
        [SerializeField] private List<Enemy> enemyPrefabs;
        [SerializeField] private Transform tankPoint;
        [SerializeField] private List<Transform> enemyPoints;
        [SerializeField][Min(0)] private int enemiesCount;
        [Space(10)]
        [SerializeField][Min(0)] private ProgressBar hPProgressBar;

        public static GameManager Singleton;
        public static Tank.Tank Tank;
        private List<Enemy> EnemyList = new List<Enemy>();
        private Coroutine generateEnemiesCoroutine;
        private bool gameIsRunning;

        void Start()
        {
            gameIsRunning = true;
            if (Singleton == null)
            {
                Singleton = this;
            }
            if (Tank == null)
            {
                Tank = Instantiate(tankPrefab);
                Tank.HealtPercentRX.Subscribe(x =>
                {
                    hPProgressBar.SetValue(x);
                }).AddTo(Tank.gameObject);
            }

            StartGame();
        }


        public void StartGame()
        {
            Clear();
            Tank.transform.position = tankPoint.position;
            Tank.gameObject.SetActive(true);
            Tank.ResetState();
            generateEnemiesCoroutine = StartCoroutine(GenerateEnemies());
        }

        private void Clear()
        {
            if(generateEnemiesCoroutine!=null){StopCoroutine(generateEnemiesCoroutine);}
            EnemyList?.ForEach(x=>Destroy(x.gameObject));
            EnemyList?.Clear();
        }

        IEnumerator GenerateEnemies()
        {
            while (gameIsRunning)
            {
                if (EnemyList.Count < enemiesCount)
                {
                    var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
                    var point = enemyPoints[Random.Range(0, enemyPoints.Count)];
                    enemy.transform.position = point.position;
                    enemy.transform.SetParent(point);
                    enemy.SetGoal(Tank.transform);
                    EnemyList.Add(enemy);
                    enemy.Destroyed.AsObservable().Subscribe(_ =>
                    {
                        if (EnemyList != null && EnemyList.Count > 0 && EnemyList.Exists(x => x == enemy))
                        {
                            EnemyList.Remove(enemy);
                        }
                    }).AddTo(enemy.gameObject);
                }
                yield return new WaitForSeconds(1f);
            }

        }
    }
}
