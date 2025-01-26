using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager;
using Util;
using Random = UnityEngine.Random;

public class pre_move : MonoBehaviour
{
    public float forceAmount = 20f;
    public float rotateForce = 1f;

    private Rigidbody rb;
    public ParticleSystem dieParticle;

    public StatusValue<float> velocityLimit = new(-5, 5);
    public Rigidbody rbX;
    public Rigidbody rbXMinus;
    public Rigidbody rbY;
    public Rigidbody rbYMinus;

    private CancellationTokenSource rotateCancelToken = new();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("rb is null");
        }
        gameObject.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(0.45f, 0.75f), gameObject.transform.localPosition.z);
        gameObject.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        rb.AddTorque(Vector3.forward * rotateForce);

        var collider = gameObject.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, rbX.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(collider, rbXMinus.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(collider, rbY.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(collider, rbYMinus.GetComponent<Collider>(), true);
    }

    private void Update()
    {
        AutoMove(transform.right * Random.Range(0.7f,1.3f));
    }

    public void OnCollisionEnter(Collision other)
    {
        CalReflection(other);
        if ((LayerMask.GetMask("Bubble Attacker") & (1 << other.gameObject.layer)) != 0)
        {
            dieParticle.gameObject.SetActive(true);
            dieParticle.transform.position = transform.position;
            dieParticle.Play();

            var gameManager = FindObjectOfType<GameManager>();
            gameManager.playerDieAction?.Invoke();
            Destroy(gameObject);
            Debug.Log("버블 사망");
            return;
        }
        
        void CalReflection(Collision other)
        {
            // 충돌 지점과 법선 벡터 가져오기 (첫 번째 접촉점 기준)
            ContactPoint contact = other.contacts[0];
            Vector3 normal = contact.normal; // 충돌 면의 법선 벡터

            // 입사 벡터 계산 (현재 속도)
            Vector3 incomingVelocity = rb.velocity;

            // 반사 벡터 계산
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);

            // 회전 축 계산: 입사 벡터와 반사 벡터의 외적 (법선 벡터에 수직)
            Vector3 rotationAxis = Vector3.Cross(incomingVelocity, reflectedVelocity).normalized;

            // 회전 속도 계산: 반사 벡터의 크기와 회전 속도 조정값에 따라 설정
            float angularSpeed = reflectedVelocity.magnitude * Random.Range(0.8f, 1.2f);

            // Rigidbody의 angularVelocity 설정
            rb.angularVelocity = rotationAxis * angularSpeed;
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (Mathf.Abs(rb.angularVelocity.magnitude) < 0.001f)
        {
            rotateCancelToken.Cancel();
            rotateCancelToken.Dispose();
            rotateCancelToken = new();
            RotateTask(rotateCancelToken.Token).Forget();
            rb.AddTorque(Vector3.forward * rotateForce);
            float clampedSpeed = Mathf.Clamp(rb.angularVelocity.magnitude, -rotateForce, rotateForce);
            rb.angularVelocity = rb.angularVelocity.normalized * clampedSpeed;
        }
    }
    void AutoMove(Vector2 dir)
    {
        rbX.AddForce((dir.x > 0 ? 1f : 1.2f) * forceAmount * dir);
        rbXMinus.AddForce( (dir.x < 0 ? 1f : 1.2f) * forceAmount * dir);
        rbY.AddForce((dir.y > 0 ? 1f : 1.2f) * forceAmount * dir);
        rbYMinus.AddForce((dir.y < 0 ? 1f : 1.2f) * forceAmount * dir);
        
        LimitRBVel(rbX);
        LimitRBVel(rbXMinus);
        LimitRBVel(rbY);
        LimitRBVel(rbYMinus);
        return;

        void LimitRBVel(Rigidbody rbVel)
        {
            float clampedSpeed = Mathf.Clamp(rbVel.velocity.magnitude, velocityLimit.Min,velocityLimit.Max);
            rbVel.velocity = rbVel.velocity.normalized * clampedSpeed;
        }
    }

    async UniTask RotateTask(CancellationToken token)
    {
        float t = 0f;
        while (t < 1f && !token.IsCancellationRequested)
        {
            await UniTask.Yield();
            t = Time.deltaTime;
            gameObject.transform.Rotate(new Vector3(0,0,90f * Time.deltaTime));
        }
    }
}
