using UnityEngine;

namespace Unite
{
    [CreateAssetMenu(fileName = "ChasePlayerAction", menuName = "Unite/Scriptable Objects/AI/Actions/ChasePlayerAction")]
    public class ChasePlayerAction : Action
    {
        private PrototypeEnemyStateMachine enemy;

        public override void ExecuteAction(IStateMachine stateMachine)
        {
            if (enemy == null)
            {
                enemy = stateMachine as PrototypeEnemyStateMachine;
            }
            enemy.Agent.SetDestination(enemy.Target.transform.position);
        }
    }
}