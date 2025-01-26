using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Gasburner : MonoBehaviour
{
    public GasLever gasLever;
    [SerializeField] private ParticleSystem Tick;
    [SerializeField] private AudioSource tickSound;
    [SerializeField] private ParticleSystem Fire;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private ParticleSystem emergency;
    
    [SerializeField] private Collider Collider;
    [SerializeField] private float MinTickTime;
    [SerializeField] private float MaxTickTime;
    [SerializeField] private int FireTickAmount;
    [SerializeField] private float PatterRateModifier;

    private bool isBurn;
    private int currentTick;
    private float patternRate;
    private IEnumerator coActiveTick;

    private void OnEnable()
    {
        Tick.Stop();
        Fire.Stop();
        Collider.gameObject.SetActive(false);
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
            tickSound.Play();

            if (FireTickAmount - 1 == currentTick)
            {
                if(!gasLever.gimmickMaterialControl.HasMaterial) gasLever.gimmickMaterialControl.AddMaterial();
                emergency.Play();
            }
        }

        Debug.Log("Fire");
        if(!gasLever.gimmickMaterialControl.HasMaterial) gasLever.gimmickMaterialControl.AddMaterial();
        Fire.Play();
        fireSound.Play();
        isBurn = true;
        Collider.gameObject.SetActive(true);
        SetActiveTick(false);
    }

    public void InteractLever()
    {
        if (isBurn)
        {
            On();
        }
        else if (FireTickAmount - 1 <= currentTick)
        {
            On();
        }

        void On()
        {
            gasLever.gimmickMaterialControl.RemoveMaterial();
            Fire.Stop();
            emergency.Stop();
            isBurn = false;
            currentTick = 0;
            Collider.gameObject.SetActive(false);
            SetActiveTick(true);
        }
    }
}
