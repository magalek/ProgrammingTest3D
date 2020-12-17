using UnityEngine;

namespace Entities {
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] private int health;
        private int Health {
            get => health;
            set {
                if (value == health) 
                    return;
            
                health = value;
                OnHealthChanged();
            }
        }

        private float HealthPercentage => (float)health / maxHealth;
        private int maxHealth;

        private MeshRenderer renderer;
    
        private void Awake() {
            maxHealth = health;
            renderer = GetComponentInChildren<MeshRenderer>();
        }

        private void OnHealthChanged() {
            if (Health <= 0) {
                renderer.material.color = Color.red;
            }
        
            if (HealthPercentage <= .5f) {
                renderer.material.color = Color.yellow;
            }
        }

        public void Damage(int amountOfDamage) {
            Health -= amountOfDamage;
            if (Health <= 0) {
                Kill();
            }
        }

        public void Kill() {
            Destroy(gameObject);
        }
    }
}


