using Entities;
using UnityEngine;

namespace Consumables {
    public class ConsumablePowerUp : MonoBehaviour {
        [SerializeField] private PowerUpType type;
        [SerializeField] private int duration;
    
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                var player = other.gameObject.GetComponent<Player>();
                AddPower(player);
                Destroy(gameObject);
            }
        }
    
        private void AddPower(Player player) {
            switch (type) {
                case PowerUpType.Movement: 
                    player.movement.PowerUp(duration);
                    break;
                case PowerUpType.Damage: 
                    player.combat.PowerUp();
                    break;
            }
        }
    }
}
