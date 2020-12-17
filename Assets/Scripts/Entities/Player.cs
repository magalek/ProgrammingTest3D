using System;
using TMPro;
using UnityEngine;

namespace Entities {
    public class Player : MonoBehaviour {

        public event Action StatusChange;

        [SerializeField] private TextMeshProUGUI stateLabel;
    
        [HideInInspector] public PlayerMovement movement; 
        [HideInInspector] public PlayerCombat combat;

        private PlayerState state;
        private int stateNum;

        private void Awake() {
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>();
            StatusChange += OnStatusChange;
        }

        public void ChangeState(int playerState) {
            stateNum += playerState;
            StatusChange?.Invoke();
        }

        private void OnStatusChange() {
            stateNum = Mathf.Clamp(stateNum, 0, 3);
            state = (PlayerState) stateNum;
            stateLabel.text = $"Current State: {state}";
        }
    }
}
