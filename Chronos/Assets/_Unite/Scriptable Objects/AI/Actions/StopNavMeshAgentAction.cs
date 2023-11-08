using UnityEngine;

namespace Unite
{
    [CreateAssetMenu(fileName ="StopNavMeshAgent", menuName ="Unite/Scriptable Objects/AI/Actions/StopNavMeshAgent")]
    public class StopNavMeshAgentAction : Action
    {
        [SerializeField]
        private bool isStopped;

        public override void ExecuteAction(IStateMachine stateMachine)
        {
            EnemyStateMachine enemy = stateMachine as EnemyStateMachine;
            enemy.Agent.isStopped = isStopped;
        }
    }
}

