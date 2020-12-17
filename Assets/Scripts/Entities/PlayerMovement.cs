using System;
using System.Collections;
using Consumables;
using UnityEngine;
using Utility;

namespace Entities {
    public class PlayerMovement : MonoBehaviour, IPowerUpable {
        public event Action StartedMoving;
        public event Action StoppedMoving;
    
        [SerializeField] private float movementSpeed;
        [SerializeField] private float glideSpeed;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldownDuration;
        [SerializeField] private float jumpForce;

        public bool IsMoving { get; private set; }
        public bool IsGrounded { get; private set; }
    
        private event Action SpacePressed;
        private event Action ShiftPressed;

        private Player player;
        private PlayerCamera playerCamera;
        private Rigidbody rigidbody;
        private GroundChecker groundChecker;

        private Timer dashTimer;
    
        private float gravityMultiplier = 6f;

        private Vector3 movementDirection;
        private float currentSpeed;

        private Vector3 initialPosition;

        private void Awake() {
            player = GetComponent<Player>();
            rigidbody = GetComponent<Rigidbody>();
            groundChecker = GetComponentInChildren<GroundChecker>();
            playerCamera = Camera.main.GetComponent<PlayerCamera>();

            initialPosition = transform.position;
            
            dashTimer = new Timer(this, dashCooldownDuration);
        
            groundChecker.TouchedGround += OnTouchedGround;
            groundChecker.LeftGround += OnLeftGround;

            SpacePressed += Jump;
            ShiftPressed += Dash;
        
            currentSpeed = movementSpeed;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SpacePressed?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                ShiftPressed?.Invoke();
            }
        }
    
        private void FixedUpdate() {
            Move();
            Rotate();
            IncreaseFallingVelocity();
        }

        public void PowerUp(float duration) {
            StartCoroutine(PowerUpCoroutine());
        
            IEnumerator PowerUpCoroutine() {
                float tempMovementSpeed = movementSpeed;
                float tempGlideSpeed = glideSpeed;
                float tempJumpForce = jumpForce;
            
                movementSpeed *= 1.5f;
                glideSpeed *= 1.5f;
                jumpForce *= 1.5f;
        
                yield return new WaitForSeconds(duration);
            
                movementSpeed = tempMovementSpeed;
                glideSpeed = tempGlideSpeed;
                jumpForce = tempJumpForce;
            }
        }

        private void Move() {

            float verticalAxis = Input.GetAxisRaw("Vertical");
            float horizontalAxis = Input.GetAxisRaw("Horizontal");

            if (verticalAxis != 0 || horizontalAxis != 0) {
                if (IsMoving == false) {
                    StartedMoving?.Invoke();
                    player.ChangeState((int)PlayerState.Moving);
                }
                IsMoving = true;
            
                var transformCache = transform;
                Vector3 forward = transformCache.forward;
                Vector3 right = transformCache.right;
            
                Vector3 movementVector = (forward * verticalAxis) + (right * horizontalAxis);
            
                movementVector.Normalize();
                movementDirection = movementVector;
                rigidbody.AddForce(movementVector * currentSpeed, ForceMode.Impulse);
            }
            else {
                if (IsMoving) {
                    StoppedMoving?.Invoke();
                    player.ChangeState(-(int)PlayerState.Moving);
                }
                IsMoving = false;
            }
        }

        private void Jump() {
            if (IsGrounded) {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
    
        private void Dash() {
            if (!dashTimer.IsRunning && IsMoving) {
                dashTimer.Start();
                rigidbody.AddForce(movementDirection * dashForce, ForceMode.Impulse);
            }
        }

        private void IncreaseFallingVelocity() {
            if (rigidbody.velocity.y < 1 && !IsGrounded) {
                rigidbody.velocity += Vector3.up * (Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime);
            }

            if (IsGrounded) {
                rigidbody.mass = 0.5f;
            }
        }

        private void Rotate() {
            rigidbody.MoveRotation(Quaternion.Euler(playerCamera.HorizontalRotation));
        }

        private void OnTouchedGround() {
            IsGrounded = true;
            currentSpeed = movementSpeed;
        }

        private void OnLeftGround() {
            IsGrounded = false;
            currentSpeed = glideSpeed;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Kill Zone")) {
                player.transform.position = initialPosition;
            }
        }

        private void OnDestroy() {
            groundChecker.TouchedGround -= OnTouchedGround;
            groundChecker.LeftGround -= OnLeftGround;
            SpacePressed -= Jump;
        }
    }
}
