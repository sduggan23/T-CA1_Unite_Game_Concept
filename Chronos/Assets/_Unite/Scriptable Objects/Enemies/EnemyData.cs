using System.Collections.Generic;
using UnityEngine;

namespace Unite
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Unite/Scriptable Objects/Enemies/Base Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField]
        private float baseHealth;

        [SerializeField]
        private float baseDamage;

        [Header("Target Detection Configuration")]
        [SerializeField]
        private EnemyDetectionData detectionData;

        [Header("Attack Configuration")]
        [SerializeField]
        private List<AttackData> attacks;

        [Header("State Machine Configuration")]
        [SerializeField]
        private State startingState;

        [SerializeField]
        private State remainState;

        public State StartState => startingState;
        public State RemainState => remainState;

        public EnemyDetectionData DetectionData => detectionData;

        public virtual void SetupEnemy(ISetupEnemy enemy)
        {
            enemy.SetupHealth(baseHealth);
            enemy.SetupBaseDamage(baseDamage);
            enemy.SetupAttacks(attacks);
            enemy.SetupStateMachine(this);
            enemy.SetupDetection(detectionData);
        }
    }
}