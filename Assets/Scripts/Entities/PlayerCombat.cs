using Consumables;
using UnityEngine;

namespace Entities {
    public class PlayerCombat : MonoBehaviour, IPowerUpable {
        [SerializeField] private Weapon leftWeapon;
        [SerializeField] private Weapon rightWeapon;

        private float damageMultiplier = 1;
    
        public void PowerUp(float duration = 0) {
            damageMultiplier += 1f;
        }

        private void Update() {
            if (Input.GetButton("Fire1")) {
                leftWeapon.Fire(damageMultiplier);
            }
            if (Input.GetButton("Fire2")) {
                rightWeapon.Fire(damageMultiplier);
            }
        }
    }
}