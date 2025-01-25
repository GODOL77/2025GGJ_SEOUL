using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private bool isPlaneHit = false;
    private bool isWallHit = false;
    private bool canJump = false;

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

    if (Input.GetKeyDown(KeyCode.J))
    {
        ApplyInitialForce();
    }

    if (canJump == true)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyInitialForce();
        }
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
            Bubble_Addforce(new Vector3(0, 1, 0)); // 위로가는 힘 적용
            Bubble_Addforce(new Vector3(1, 0, 0)); // 앞으로 가는 힘 적용
        }
    }

    async Task CheckPlaneHit()
    {
        if (isPlaneHit == true)
        {
            canJump = true;
            await Task.Delay(50);   // 50ms 대기
            Debug.Log("50ms 대기했음");
            canJump = false;
            isPlaneHit = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter 인식됨");
        if (other.gameObject.CompareTag("Plane"))
        {
            Debug.Log("방울이 바닥에 닿음");
            isPlaneHit = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 수직 속도 초기화
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse); // 위로 튕기는 힘 추가
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            isWallHit = true;
        }
    }

    void CheckHitWall()
    {
        if (isWallHit == true)
        {

        }
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