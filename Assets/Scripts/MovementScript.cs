using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // SerializedField umožňuje upravovat proměnou v editoru bez toho aby byla public
    [SerializeField] Transform playerCamera;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float speed = 6.0f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask ground;

    public float jumpForce = 500f;
    bool isGrounded;

    float cameraCap; 
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouse(); // pohyb kamery

        // bool pokud je hráč na zemi (podmínka pro skok)
        isGrounded = Physics.CheckSphere(GroundCheck.position, 0.2f, ground, QueryTriggerInteraction.Ignore);

    }
    // pro fyzické kalkulace se používá fixedUpdate, volá se 50 krát za 1 sekundu
    private void FixedUpdate()
    {
        UpdateMove(); // pohyb postavy hráče
    }

    void UpdateMouse()
    {
        // Vektor s aktualní pozicí kursoru (kam chceme posunout kameru)
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // plynulý přechod z current do target pozice
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmothTime);
        // limit rotace kamery (koukání nahorů a dolů)
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);
        // rotace kamery
        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        // rotace postavy
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);


    }

    void UpdateMove()
    {
        // směr vstupů (klávesy wsad)
        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        targetDir = transform.TransformDirection(targetDir) * speed; // transformace z lokálního prostoru na celkový prostor
        //Debug.Log(isGrounded);
        if (isGrounded && Input.GetButtonDown("Jump")) // Jump = Space
        {
            
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // přidaní síly směrující nahoru na rigidbody
        }

        rb.MovePosition(rb.position + targetDir * Time.fixedDeltaTime); // hýbe rigidbody do směru chůze


    }


}
