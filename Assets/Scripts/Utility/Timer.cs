using System;
using System.Collections;
using UnityEngine;

namespace Utility {
    public class Timer {
        public event Action Started;
        public event Action Ended;

        public bool IsRunning { get; private set; }
    
        private readonly MonoBehaviour owner;
        private readonly float duration;

        private Coroutine coroutine;
    
        public Timer(MonoBehaviour owner, float duration) {
            this.owner = owner;
            this.duration = duration;
        }

        public void Start() {
            coroutine = owner.StartCoroutine(CooldownCoroutine());
        }

        public void End() {
            if (coroutine == null) return;
            owner.StopCoroutine(coroutine);
        }

        private IEnumerator CooldownCoroutine() {
            Started?.Invoke();
            IsRunning = true;
            yield return new WaitForSeconds(duration);
            Ended?.Invoke();
            IsRunning = false;
        }
    }
}