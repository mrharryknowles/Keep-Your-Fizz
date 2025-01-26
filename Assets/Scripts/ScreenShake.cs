using UnityEngine;

public class ScreenShake : MonoBehaviour {
    private Transform cam;
    public AnimationCurve shakeScale;

    private float shookAt;
    private float currentShakeScale;
    private float currentShakeDuration;

    
    private void Start() {
        cam = Camera.main.transform;
    }

    private void Update() {
        float t = (Time.time-shookAt)/currentShakeDuration;
        if (t < 1) {
            Vector2 pos = Random.insideUnitCircle * shakeScale.Evaluate(t) * currentShakeScale;
            cam.transform.position = new Vector3(pos.x, pos.y, -10);
        } else {
            cam.transform.position = new Vector3(0, 0, -10);
        }
    }

    public void ShakeScreen(float amplitude, float duration) {
        shookAt = Time.time;
        currentShakeScale = amplitude;
        currentShakeDuration = duration;
    }

    public void ShakeScreen(Amount amount) {
        ShakeScreen(amount.amplitude, amount.duration);
    }

    [System.Serializable]
    public class Amount {
        public float amplitude;
        public float duration;
    }
}