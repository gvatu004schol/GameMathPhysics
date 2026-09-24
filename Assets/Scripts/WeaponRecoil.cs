using UnityEngine;
using UnityEngine.InputSystem;   // <-- new Input System

/// <summary>
/// Toy project: weapon recoil driven by EasingFunction.
/// Pick the easing curve per weapon in the Inspector — no code changes needed.
/// Press Space to fire. Each weapon kicks back + tilts up, then eases back to rest.
/// </summary>
public class WeaponRecoil : MonoBehaviour
{

    [System.Serializable]
    public class Weapon
    {
        public string Name = "Weapon";
        public Transform Barrel;                         // the object we animate
        public EasingFunction.Ease Easing = EasingFunction.Ease.EaseOutCubic;

        [Header("Recoil pose (relative to rest)")]
        public Vector3 KickOffset = new Vector3(0f, 0f, -0.5f);   // push back
        public Vector3 KickRotation = new Vector3(-25f, 0f, 0f);  // tilt up

        [Header("Timing")]
        [Range(0.05f, 1f)] public float KickDuration = 0.08f;    // fast kick out
        [Range(0.1f, 3f)] public float ReturnDuration = 0.9f;    // eased return

        // runtime
        [HideInInspector] public Vector3 RestPos;
        [HideInInspector] public Quaternion RestRot;
    }

    public Weapon[] Weapons;

    [Range(1f, 30f)] public float FireCooldown = 0.35f;

    private EasingFunction.Function[] returnFuncs;
    private float nextFireTime;

    void Start()
    {
        int n = Weapons.Length;
        returnFuncs = new EasingFunction.Function[n];

        for (int i = 0; i < n; i++)
        {
            var w = Weapons[i];
            if (w.Barrel == null)
            {
                Debug.LogError($"Weapon '{w.Name}' has no Barrel assigned.");
                enabled = false;
                return;
            }
            w.RestPos = w.Barrel.localPosition;
            w.RestRot = w.Barrel.localRotation;
            returnFuncs[i] = EasingFunction.GetEasingFunction(w.Easing);

            if (returnFuncs[i] == null)
                Debug.LogError($"No easing for {w.Easing} on '{w.Name}'");
        }
    }

    void Update()
    {
        // --- New Input System ---
        // Keyboard.current can be null if no keyboard is present (rare on desktop).
        bool firePressed =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed;

        if (firePressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + FireCooldown;
            for (int i = 0; i < Weapons.Length; i++)
                StartCoroutine(FireWeapon(Weapons[i], returnFuncs[i]));
        }
    }

    System.Collections.IEnumerator FireWeapon(Weapon w, EasingFunction.Function ease)
    {
        // ---- Phase 1: snap into recoil pose (linear, fast) ----
        Vector3 recoilPos = w.RestPos + w.KickOffset;
        Quaternion recoilRot = w.RestRot * Quaternion.Euler(w.KickRotation);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / w.KickDuration;
            float k = Mathf.Clamp01(t);
            w.Barrel.localPosition = Vector3.Lerp(w.RestPos, recoilPos, k);
            w.Barrel.localRotation = Quaternion.Slerp(w.RestRot, recoilRot, k);
            yield return null;
        }

        // ---- Phase 2: eased return to rest ----
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / w.ReturnDuration;
            float k = Mathf.Clamp01(t);

            // The easing function drives the blend — this is the part the assignment cares about.
            float e = ease(0f, 1f, k);

            w.Barrel.localPosition = Vector3.LerpUnclamped(recoilPos, w.RestPos, e);
            w.Barrel.localRotation = Quaternion.SlerpUnclamped(recoilRot, w.RestRot, e);

            yield return null;
        }

        // Snap exactly to rest so drift can't accumulate.
        w.Barrel.localPosition = w.RestPos;
        w.Barrel.localRotation = w.RestRot;
    }
}