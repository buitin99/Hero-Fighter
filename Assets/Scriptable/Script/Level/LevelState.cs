using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelState", menuName = "Scriptable/LevelState")]
public class LevelState : ScriptableObject
{
    public string nameLevel;
    public List<Wave> waves = new List<Wave>();



    [Serializable]
    public class Wave
    {
        public string name;
        public List<Enemy> enemies = new List<Enemy>();
    }

    [Serializable]
    public class Enemy
    {
        public GameObjectPool enemyObjectPool;
        public Vector3 position;
        public Vector3 eulerRotation;
        public Quaternion rotation { 
            get { 
                return Quaternion.Euler(eulerRotation);
            }
        }
        public Enemy(GameObjectPool enemy)
        {
            enemyObjectPool = enemy;
        }
    }

}