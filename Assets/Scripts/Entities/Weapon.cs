using Bullets;
using UnityEngine;
using Utility;

namespace Entities {
    public class Weapon : MonoBehaviour {
        [SerializeField] [Range(0f, 5f)] private float fireCooldownDuration;
        [SerializeField] private Bullet bulletPrefab;

        private Timer fireCooldown;
    
        private void Awake() {
            fireCooldown = new Timer(this, fireCooldownDuration);
        }

        public void Fire(float damageMultiplier) {
            if (fireCooldown.IsRunning)
                return;

            float force = bulletPrefab.Speed;
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Damage = (int)(bullet.Damage * damageMultiplier);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 50 * force, ForceMode.Impulse);
            fireCooldown.Start();
        }
    }
}