using System.Collections;
using UnityEngine;

namespace Unite
{
    public class BasicPistol : MonoBehaviour, IDoDamage
    {
        [SerializeField] private Camera cam;

        [SerializeField] private float range = 100f;
        [SerializeField] private float damage = 25f;

        [SerializeField] private float timeBetweenShots = 0.5f;
        private bool canShoot = true;

        [SerializeField] private ParticleSystem muzzleFlashEffect;

        private PlayerInputActions inputActions;
        private PlayerInputActions.DefaultActions defaultActions;

        private void Awake()
        {
            inputActions = new PlayerInputActions();
            defaultActions = inputActions.Default;

            defaultActions.Shoot.performed += ctx =>
            {
                if (canShoot)
                    StartCoroutine(Shoot());
            };
        }

        private IEnumerator Shoot()
        {
            Debug.Log("Invoking Shoot coroutine");

            canShoot = false;

            while (defaultActions.Shoot.IsPressed())
            {
                PlayMuzzleFlash();
                FireRaycast();
                yield return new WaitForSeconds(timeBetweenShots);
            }

            canShoot = true;
        }

        private void PlayMuzzleFlash()
        {
            if (muzzleFlashEffect == null) return;
            muzzleFlashEffect.Play();
        }

        private void FireRaycast()
        {
            RaycastHit hit;
            bool raycast = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);

            if (!raycast) return;
            Debug.Log("Shot " + hit.transform.gameObject.name);

            DoDamage(hit.transform.gameObject);
        }

        private void OnEnable()
        {
            canShoot = true; // prevents shooting from being locked after switching to a new weapon
            defaultActions.Enable();
        }

        private void OnDisable()
        {
            defaultActions.Disable();
        }

        public void DoDamage(GameObject target)
        {
            ITakeDamage damageable = target.transform.GetComponent<ITakeDamage>();
            if (damageable == null) return;

            Debug.Log(damageable.ToString());
            damageable.TakeDamage(damage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(cam.transform.position, cam.transform.forward * range);
        }
    }
}