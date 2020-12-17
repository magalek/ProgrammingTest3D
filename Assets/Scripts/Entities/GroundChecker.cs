using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities {
    public class GroundChecker : MonoBehaviour {
        public event Action TouchedGround; 
        public event Action LeftGround;

        private Player player;

        private void Awake() {
            player = GetComponentInParent<Player>();
        }

        private List<Collider> currentColliders = new List<Collider>();

    
        private void OnTriggerEnter(Collider other) {
            if (!other.gameObject.CompareTag("Player") && !other.isTrigger) {
                if (currentColliders.Count == 0) {
                    TouchedGround?.Invoke();
                    player.ChangeState(-(int)PlayerState.InAir);
                }
                currentColliders.Add(other);
            }
        }
    
        private void OnTriggerExit(Collider other) {
            if (!other.gameObject.CompareTag("Player") && !other.isTrigger) {
                currentColliders.Remove(other);
                if (currentColliders.Count == 0) {
                    LeftGround?.Invoke();
                    player.ChangeState((int)PlayerState.InAir);
                }
            }
        }
    }
}

