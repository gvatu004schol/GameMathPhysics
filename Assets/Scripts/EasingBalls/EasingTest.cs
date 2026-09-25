using UnityEngine;
using UnityEngine.UIElements;

public class EasingTest : MonoBehaviour
{

    [System.Serializable]
    public class BallTween
    {
        public GameObject Ball;
        public EasingFunction.Ease Easing;   // dropdown in the Inspector
        public Color StartColor = Color.white;
        public Color EndColor = Color.red;
        [Tooltip("How far right (world X) this ball travels.")]
        public float MoveAmount = 5f;
    }

    public BallTween[] Balls;

    [Range(1f, 20f)] public float EasingTime = 3f;
    [Range(0f, 10f)] public float StartTime = 1f;
    public bool Loop = true;

    private Vector3[] originalPositions;
    private Material[] materials;
    private EasingFunction.Function[] easeFuncs;

    void Start()
    {
        int n = Balls.Length;
        originalPositions = new Vector3[n];
        materials = new Material[n];
        easeFuncs = new EasingFunction.Function[n];

        for (int i = 0; i < n; i++)
        {
            originalPositions[i] = Balls[i].Ball.transform.position;
            materials[i] = Balls[i].Ball.GetComponent<MeshRenderer>().material;
            easeFuncs[i] = EasingFunction.GetEasingFunction(Balls[i].Easing);

            if (easeFuncs[i] == null)
                Debug.LogError($"No easing function for {Balls[i].Easing} on ball {i}");
        }
    }

    void Update()
    {
        if (Time.time < StartTime) return;

        float elapsed = Time.time - StartTime;
        float t = Loop
            ? Mathf.Clamp01((elapsed % EasingTime) / EasingTime)
            : Mathf.Clamp01(elapsed / EasingTime);

        for (int i = 0; i < Balls.Length; i++)
        {
            var b = Balls[i];
            float e = easeFuncs[i](0f, 1f, t);   // normalized value [0,1] for color

            // Position — the easing function does the lerp for us:
            //   from originalPositions[i] to originalPositions[i] + right*MoveAmount
            Vector3 targetPos = originalPositions[i] + b.MoveAmount * Vector3.right;
            float px = easeFuncs[i](originalPositions[i].x, targetPos.x, t);
            float py = originalPositions[i].y;
            float pz = originalPositions[i].z;
            b.Ball.transform.position = new Vector3(px, py, pz);

            // Color — use the same function, mapped to [0,1]
            materials[i].color = Color.Lerp(b.StartColor, b.EndColor, e);
        }
    }
}