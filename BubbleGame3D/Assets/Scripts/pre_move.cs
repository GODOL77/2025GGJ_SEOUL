using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class pre_move : MonoBehaviour
{
    public float forceAmount = 20f;
    public float bounceForce = 10f;
    public int MaxRandomAmount = 10;
    public int MinRandomAmount = 1;

    private float jumpForceMultiplier = 3.5f; // 점프 힘 감소율
    private float minJumpForce = 1.0f;  // 최소 점프 힘
    private float maxJumpForce = 10.0f; // 최대 점프 힘
    private float currentJumpForce;   // 현재 점프 힘 저장

    private Rigidbody rb;
    public Rigidbody top_rb;
    public Rigidbody left_rb;
    public Rigidbody right_rb;
    public Rigidbody down_rb;
    public GameObject Bubble;
    public ParticleSystem dieParticle;

    private bool canJump = false; // 점프 가능 상태
    private bool right = true;
    private Coroutine jumpCoroutine;
    private float lastWallHitTime = -1f; // 마지막 벽에 닿은 시간

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("rb is null");
        }
        currentJumpForce = maxJumpForce; // 시작할 때 최대 힘 설정
    }

    private void Update()
    {
        int randomValue = Random.Range(0, 2);
        int rv = Random.Range(MinRandomAmount, MaxRandomAmount);

        if (Input.GetKeyDown(KeyCode.J))
        {
            ApplyInitialForce();
        }

        if (canJump && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ApplyJumpForce();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !canJump)
        {
            Debug.Log("점프 불가 상태");
        }

        Vector3 force = new Vector3(1, 1, 0);
        if (force.x > 5)
        {
            force.x = 5;
        }
        else if (force.x < -5)
        {
            force.x = -5;
        }
        else if (force.y > 5)
        {
            force.y = 5;
        }

        float currentHeight = transform.position.y;

        // if (Keyboard.current.wKey.wasPressedThisFrame && top_rb != null)
        // {
        //     Bubble_Addforce(new Vector3(0, 1, 0));
        // }
        // else if (Keyboard.current.aKey.wasPressedThisFrame && top_rb != null)
        // {
        //     Bubble_Addforce(new Vector3(-1, 0, 0));
        // }
        // else if (Keyboard.current.sKey.wasPressedThisFrame && top_rb != null)
        // {
        //     Bubble_Addforce(new Vector3(0, -1, 0));
        // }
        // else if (Keyboard.current.dKey.wasPressedThisFrame && top_rb != null)
        // {
        //     Bubble_Addforce(new Vector3(1, 0, 0));
        // }
    }

    void ApplyInitialForce()
    {
        if (right == true)
        {
            Bubble_Addforce(new Vector3(0.5f, 1, 0)); // 위로가는 힘 적용
        }
        else
        {
            Bubble_Addforce(new Vector3(-0.5f, 1, 0));
        }
    }

    private bool isGrounded = false; // 바닥에 닿았는지 여부

private void OnTriggerEnter(Collider collision)
{
    if ((LayerMask.GetMask("Bubble Attacker") & (1 << collision.gameObject.layer)) != 0)
    {
        dieParticle.gameObject.SetActive(true);
        dieParticle.transform.position = transform.position;
        dieParticle.Play();
        Destroy(gameObject);
        Debug.Log("버블 사망");
        return;
    }
    
    if (collision.gameObject.CompareTag("Plane") && !isGrounded)
    {
        isGrounded = true; // 바닥에 닿음
        Debug.Log("닿았음");
        currentJumpForce *= jumpForceMultiplier;
        currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);

        Bubble_Addforce(new Vector3(0, currentJumpForce, 0));
        Debug.Log($"적용된 점프 힘: {currentJumpForce}");

        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }
        jumpCoroutine = StartCoroutine(EnableJumpForLimitedTime());
    }

    if (collision.gameObject.CompareTag("Wall"))
    {
        Debug.Log("벽 닿았음");
        
        // 벽에 닿은 후 3초가 지나면 반전
        if (Time.time - lastWallHitTime > 3f)
        {
            right = !right;
            lastWallHitTime = Time.time; // 마지막 벽에 닿은 시간 갱신
            Debug.Log("반전 완료");
        }
    }
}



    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            isGrounded = false; // 바닥에서 벗어남
        }
    }

    private IEnumerator EnableJumpForLimitedTime()
    {
        canJump = true;
        yield return new WaitForSeconds(0.05f); // 50ms
        canJump = false;
    }

    void ApplyJumpForce()
    {
        Vector3 jumpDirection = right ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);
        rb.AddForce(jumpDirection * forceAmount, ForceMode.Impulse);
    }
    void Bubble_Addforce(Vector3 dir)
    {
        int Select_Sub = Random.Range(0, 2);
        int Sub_Value = Random.Range(MinRandomAmount, MaxRandomAmount);
        float Dir_X = dir.x;
        float Dir_Y = dir.y;
        if (Dir_Y > 0)
        {
            top_rb.AddForce(new Vector3(0, 1, 0) * forceAmount, ForceMode.Impulse);
            if (Select_Sub == 1)
                left_rb.AddForce(new Vector3(0, 1, 0) * Sub_Value, ForceMode.Impulse);
            else
                right_rb.AddForce(new Vector3(0, 1, 0) * Sub_Value, ForceMode.Impulse);
        }
        else if (Dir_Y < 0)
        {
            down_rb.AddForce(new Vector3(0, -1, 0) * forceAmount, ForceMode.Impulse);
            if (Select_Sub == 1)
                left_rb.AddForce(new Vector3(0, -1, 0) * Sub_Value, ForceMode.Impulse);
            else
                right_rb.AddForce(new Vector3(0, -1, 0) * Sub_Value, ForceMode.Impulse);
        }

        if (Dir_X < 0)
        {
            left_rb.AddForce(new Vector3(-1, 0, 0) * forceAmount, ForceMode.Impulse);

            if (Select_Sub == 1)
                top_rb.AddForce(new Vector3(-1, 0, 0) * Sub_Value, ForceMode.Impulse);
            else
                down_rb.AddForce(new Vector3(-1, 0, 0) * Sub_Value, ForceMode.Impulse);
        }
        else if (Dir_X > 0)
        {
            right_rb.AddForce(new Vector3(1, 0, 0) * forceAmount, ForceMode.Impulse);
            if (Select_Sub == 1)
                top_rb.AddForce(new Vector3(1, 0, 0) * Sub_Value, ForceMode.Impulse);
            else
                down_rb.AddForce(new Vector3(1, 0, 0) * Sub_Value, ForceMode.Impulse);
        }
    }
}
