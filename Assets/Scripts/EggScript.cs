using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EggScript : MonoBehaviour
{
    public Sprite cracked;
    public GameManager gm;
    private bool isCracked, finished, isTimeReducedPan, isOnPan, isCheckingOnPan;
    private float onPanReduceTime;
    public float floorAngleSign;
    public bool isGrounded, notFlat, zoomedOut;
    public int crackThreshold, speed;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    AudioSource crackSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        crackSound = GetComponent<AudioSource>();

        isCracked = false;
        notFlat = false;
        finished = false;
        isGrounded = true;
        isTimeReducedPan = false;
        isCheckingOnPan = false;
        zoomedOut = false;
        floorAngleSign = 0;
        onPanReduceTime = 5;
    }

    private void FixedUpdate()
    {
        if (!finished)
        {
            if (!isCracked)
            {
                MoveEgg();
            }
            else
            {
                StartCoroutine(Respawn());
            }
        }
    }

    public virtual void MoveEgg()
    {
        rb.AddTorque(Input.GetAxis("Horizontal") * speed * -1);
        if (notFlat)
        {
            rb.AddForce(new Vector2(3, 3 * floorAngleSign) * Input.GetAxis("Horizontal"));
        }
        if (!isGrounded)
        {
            rb.AddForce(new Vector2(2, 0) * Input.GetAxis("Horizontal"));
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        if (!finished)
        {
            gm.RestartLevel();
        }
    }

    IEnumerator ZoomOut()
    {
        while (Camera.main.orthographicSize < 18)
        {
            Camera.main.orthographicSize += 10 * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ZoomIn()
    {
        while(Camera.main.orthographicSize > 8)
        {
            Camera.main.orthographicSize -= 10 * Time.deltaTime;
            yield return null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (zoomedOut && !collision.gameObject.CompareTag("Spring") && Mathf.Abs(collision.gameObject.transform.rotation.eulerAngles.z) < 90)
        {
            zoomedOut = false;
            StartCoroutine(ZoomIn());
        }
        if (collision.gameObject.CompareTag("Spring"))
        {
            Spring spring = collision.gameObject.GetComponent<Spring>();
            rb.velocity = new Vector2(spring.rightForce, spring.upForce);
            if (!zoomedOut && spring.zoomOut)
            {
                zoomedOut = true;
                StartCoroutine(ZoomOut());
            }
        }
        else if (collision.relativeVelocity.magnitude >= crackThreshold && !isCracked)
        {
            if (!collision.gameObject.CompareTag("Bread") && !collision.gameObject.CompareTag("Boiler"))
            {
                sr.sprite = cracked;
                crackSound.Play();
                isCracked = true;
            }
        } 
        else if (collision.gameObject.CompareTag("Knife") && !isCracked)
        {
            sr.sprite = cracked;
            crackSound.Play();
            isCracked = true;
        }
        else
        {
            if (collision.gameObject.transform.rotation.z != 0)
            {
                notFlat = true;
                floorAngleSign = Mathf.Sign(collision.gameObject.transform.rotation.z);
            }
            else
            {
                notFlat = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !isCracked && !finished)
        {
            gm.LevelFinish(gameObject.name);
            finished = true;
        } else if (collision.gameObject.CompareTag("Fail"))
        {
            StartCoroutine(Respawn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isOnPan = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.gameObject.transform.rotation.eulerAngles.z) != 90)
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            isOnPan = true;
            if (!isTimeReducedPan && !isCheckingOnPan)
            {
                StartCoroutine(CheckOnPan());
            }
        }
    }

    IEnumerator CheckOnPan()
    {
        isCheckingOnPan = true;
        yield return new WaitForSeconds(1);
        if (isOnPan)
        {
            gm.ReduceElapsedTime(onPanReduceTime, gameObject.name);
            isTimeReducedPan = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
