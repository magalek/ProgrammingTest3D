using System;
using Entities;
using UnityEngine;
using Utility;

namespace Bullets {
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Bullet : MonoBehaviour {
        [SerializeField] private int damage = 1;
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isSplash;
        [SerializeField] private float radius = 1;
        [SerializeField] private float lifetime;

        public float Speed => speed;

        public int Damage {
            get => damage;
            set => damage = value;
        }

        private Timer lifetimeTimer;
        
        private void Awake() {
            lifetimeTimer = new Timer(this, lifetime);
            lifetimeTimer.Ended += () => Hit();
            lifetimeTimer.Start();
        }

        private void OnTriggerEnter(Collider other) {
            Hit(other);
        }

        private void Hit(Collider other = null) {
            if (other != null && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground Checker")))
                return;

            if (isSplash) {
                var collisions = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider collision in collisions) {
                    if (collision.TryGetComponent(out IDamageable damageable)) {
                        damageable.Damage(damage);
                    }
                }
            }
            else {
                if (other == null) {
                    Destroy(gameObject);
                }
                else if (other.TryGetComponent(out IDamageable damageable)) {
                    damageable.Damage(damage);
                }
            }
            Destroy(gameObject);
        }

        private void OnDestroy() {
            lifetimeTimer.End();
        }
    }
}