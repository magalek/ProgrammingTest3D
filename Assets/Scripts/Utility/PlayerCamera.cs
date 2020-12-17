using UnityEngine;

namespace Utility {
    public class PlayerCamera : MonoBehaviour {

        [SerializeField] private Transform body;

        [SerializeField] private float cameraTurnSpeed;
    
        public Vector3 HorizontalRotation { get; private set; }

        private Vector3 positionOffset;
    
        private float yRotation;
        private float xRotation;

        private void Awake() {
            positionOffset = transform.position - body.position;
        }

        private void LateUpdate() {
            HorizontalRotation = Rotate();
            FollowPlayer();
        }

        private void FollowPlayer() {
            transform.position = body.position + positionOffset;
        }

        private Vector3 Rotate() {
            yRotation = Input.GetAxis("Mouse X") * cameraTurnSpeed * Time.deltaTime;
            xRotation += Input.GetAxis("Mouse Y") * cameraTurnSpeed * Time.deltaTime;

            xRotation = Mathf.Clamp(xRotation, -60, 60);

            Transform transformCache = transform;

            transformCache.rotation = Quaternion.Euler(new Vector3(-xRotation, transformCache.eulerAngles.y + yRotation, 0));
        
            return new Vector3(0, transformCache.eulerAngles.y + yRotation, 0);
        }
    }
}
