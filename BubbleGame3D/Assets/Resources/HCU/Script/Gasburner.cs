using System.Collections;
using UnityEngine;

public class Gasburner : MonoBehaviour
{
    [SerializeField] private ParticleSystem Tick;
    [SerializeField] private ParticleSystem Fire;
    [SerializeField] private Collider Collider;
    [SerializeField] private float MinTickTime;
    [SerializeField] private float MaxTickTime;
    [SerializeField] private float FireTickAmount;
    [SerializeField] private float PatterRateModifier;

    private bool isBurn;
    private int currentTick;
    private float patternRate;
    private IEnumerator coActiveTick;

    private void OnEnable()
    {
        Tick.Stop();
        Fire.Stop();
        Collider.enabled = false;
        isBurn = false;
        currentTick = 0;
        patternRate = 0.0f;
        SetActiveTick(true);
    }

    private void OnDisable()
    {
        SetActiveTick(false);
    }

    private void SetActiveTick(bool isActive)
    {
        if (isActive)
        {
            if(coActiveTick != null) this.StopCoroutine(coActiveTick);
            coActiveTick = ActiveTick();
            this.StartCoroutine(coActiveTick);
        }
        else
        {
            this.StopCoroutine(coActiveTick);
        }
    }

    private IEnumerator ActiveTick()
    {
        while (currentTick < FireTickAmount)
        {
            float finalMaxTime = Mathf.Clamp(MaxTickTime - patternRate, MinTickTime, MaxTickTime);
            float tickTime = Random.Range(MinTickTime, finalMaxTime);
            float current = 0.0f;

            while (current < tickTime)
            {
                current += Time.deltaTime;
                patternRate += Time.deltaTime * PatterRateModifier;
                yield return null;
            }

            currentTick++;
            Debug.Log("Tick");
            Tick.Play();
        }

        Debug.Log("Fire");
        Fire.Play();
        isBurn = true;
        Collider.enabled = true;
        SetActiveTick(false);
    }

    public void InteractLever()
    {
        if (!isBurn) return;
        Fire.Stop();
        isBurn = false;
        currentTick = 0;
        Collider.enabled = false;
        SetActiveTick(true);
    }
}
