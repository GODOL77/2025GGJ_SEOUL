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
    public int MinRandomAmount = 5;
    
    private Rigidbody rb;
    public Rigidbody top_rb;
    public Rigidbody left_rb;
    public Rigidbody right_rb;
    public Rigidbody down_rb;

    private bool canJump = false; // 점프 가능 상태
    private Coroutine jumpCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("rb is null");
        }

    }


    private void Update()
    {
        int randomValue = Random.Range(0, 2);
        int rv = Random.Range(MinRandomAmount, MaxRandomAmount);

    if (Input.GetKeyDown(KeyCode.J))    // 임시 디버깅용 코드
    {
        ApplyInitialForce();
    }

    if (canJump && Keyboard.current.spaceKey.wasPressedThisFrame)
    {
        ApplyJumpForce();
    }

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
        {
            Bubble_Addforce(new Vector3(1, 1, 0)); // 위로가는 힘 적용
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            Debug.Log("닿았음");
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
            jumpCoroutine = StartCoroutine(EnableJumpForLimitedTime());
        }
    }

    private IEnumerator EnableJumpForLimitedTime()
    {
        canJump = true;
        yield return new WaitForSeconds(0.05f); // 50ms(0.05초)
        canJump = false;
    }

    private void ApplyJumpForce()
    {
        rb.AddForce(new Vector3(1, 1, 0) * forceAmount, ForceMode.Impulse);
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