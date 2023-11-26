using UnityEngine;

namespace Unite.WeaponSystem
{
    [CreateAssetMenu(fileName = "BulletTrailData", menuName = "Weapons/Bullet Trail")]
    public class BulletTrailData : ScriptableObject
    {
        [Header("TrailRenderer Configuration")]
        [SerializeField]
        private Material material;

        [SerializeField]
        private AnimationCurve widthCurve;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float minVertexDistance;

        [SerializeField]
        private Gradient color;

        [Header("Bullet Trail Configuration")]
        [SerializeField]
        private float missDistance;

        [SerializeField]
        private float simulationSpeed;

        public Material Material => material;
        public AnimationCurve WidthCurve => widthCurve;
        public float Duration => duration;
        public float MinVertexDistance => minVertexDistance;
        public Gradient Color => color;
        public float SimulationSpeed => simulationSpeed;
        public float MissDistance => missDistance;
    }
}