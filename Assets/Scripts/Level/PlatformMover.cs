using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public class PlatformMover : MonoBehaviour {
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float platformSpeed;
        [SerializeField] private float platformDelay;
    
        private Queue<Transform> waypointsQueue;

        private Transform currentWaypoint;
        private float duration;

        private Rigidbody playerRigidbody;
    
        private void Awake() {
            waypointsQueue = new Queue<Transform>(waypoints);
            StartCoroutine(MoveCoroutine());
        }

        private void Update() {
            duration += Time.deltaTime / 60 * platformSpeed;
            Vector3 offset = Vector3.zero;

            if (playerRigidbody != null) {
                offset = playerRigidbody.position - transform.position;
            }

            transform.position = Vector3.Lerp(transform.position, currentWaypoint.position, duration);
        
            if (playerRigidbody != null) {
                playerRigidbody.MovePosition(transform.position + offset);
            }
        }

        private IEnumerator MoveCoroutine() {
            while (gameObject.activeSelf) {
                currentWaypoint = waypointsQueue.Dequeue();
                yield return new WaitUntil(() => Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f);
                yield return new WaitForSeconds(platformDelay);
                duration = 0;
                waypointsQueue.Enqueue(currentWaypoint);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                playerRigidbody = other.attachedRigidbody;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                playerRigidbody = null;
            }
        }
    }
}
