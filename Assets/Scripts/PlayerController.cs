using UnityEngine;
using System.Collections;

namespace CircleBreak
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        public static event System.Action PlayerDied;

        public GameObject changeDirObject;

        public float moveForce;

        [HideInInspector]
        public bool canMove;

        public int breakDamage;
        public float newBreakDamage;

        private Vector3 moveDirection;
        Rigidbody2D body;
        Vector3 lastFrameVelocity;

        float timeCountSameUpvector;
        float timeCountSameRightVector;


        float timeDelay = 1.5f;

        float timeCountSomeMoveDir;
        float timeDelaySameMoveDir = 3f;

        float countStayBoundary;
        float countStayBreakable;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            body = GetComponent<Rigidbody2D>();
            timeCountSameUpvector = 0;
            newBreakDamage = Random.Range(0.1f, 0.5f);
        }

        void OnValidate()
        {
            if (breakDamage <= 0)
                breakDamage = 1;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Activities that take place every frame
            if (canMove)
                Movement();
            lastFrameVelocity = body.linearVelocity;
            if (Vector3.Dot(moveDirection.normalized, Vector3.right) > 0.99 && Vector3.Dot(moveDirection.normalized, Vector3.right) <= 1)
            {

                timeCountSameRightVector += Time.deltaTime;
                //			Debug.Log ("Bang voi phuong ngang, time: " + timeCountSameMoveDir + " dot: " + Vector3.Dot (moveDirection.normalized, Vector3.right));
                if (timeCountSameRightVector > timeDelay)
                {
                    //				Debug.Log ("Lon hon");
                    GameObject spawnChangeDir = Instantiate(changeDirObject);
                    spawnChangeDir.transform.position = transform.position;
                    spawnChangeDir.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    timeCountSameRightVector = 0;
                }
            }
            else
            {
                timeCountSameRightVector = 0;
            }
            if (Vector3.Dot(moveDirection.normalized, Vector3.up) > 0.99 && Vector3.Dot(moveDirection.normalized, Vector3.up) <= 1)
            {

                timeCountSameUpvector += Time.deltaTime;
                //			Debug.Log ("Bang voi phuong dung, time: " + timeCountSameMoveDir + " dot: " + Vector3.Dot (moveDirection.normalized, Vector3.up));
                if (timeCountSameUpvector > timeDelay)
                {
                    //				Debug.Log ("Lon hon");
                    GameObject spawnChangeDir = Instantiate(changeDirObject);
                    spawnChangeDir.transform.position = transform.position;
                    spawnChangeDir.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    timeCountSameUpvector = 0;
                }
            }
            else
            {
                timeCountSameUpvector = 0;
            }
        }

        public void ChangeMoveDirection(Vector3 moveDir)
        {
            moveDirection = moveDir;
        }

        void Movement()
        {

            body.linearVelocity = moveDirection.normalized * moveForce * Time.deltaTime;
        }


        // Calls this when the player dies and game over
        public void Die()
        {
            canMove = false;
            // Fire event
            if (PlayerDied != null)
                PlayerDied();
            CameraController.Instance.ShakeCamera();
            Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D target)
        {

            if (target.gameObject.tag == "CircleBreak")
            {
                Die();
            }

            if (target.contacts.Length > 0)
            {
                if (target.gameObject.tag == "Boundary")
                {
                    countStayBoundary = 0;
                    moveDirection = Vector3.Reflect(lastFrameVelocity, target.contacts[0].normal);
                }
                if (target.gameObject.tag == "Breakout")
                {
                    moveDirection = Vector3.Reflect(lastFrameVelocity, target.contacts[0].normal);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.reflect);
                }
                if (target.gameObject.tag == "BreakableTile")
                {
                    countStayBreakable = 0;
                    moveDirection = Vector3.Reflect(lastFrameVelocity, target.contacts[0].normal);
                    BreakableTile breakTile = target.gameObject.GetComponent<BreakableTile>();
                    breakTile.DecreaseNumberAndCheckDestroy(newBreakDamage);
                }
                if (target.gameObject.tag == "ChangeDirection")
                {
                    Vector3 newDir = new Vector3(Random.Range(-1.01f, 1.01f), Random.Range(-1.01f, 1.01f), 0);
                    moveDirection = Vector3.Reflect(newDir.normalized, target.contacts[0].normal);
                    Destroy(target.gameObject, 0.7f);
                }
            }
        }

        void OnCollisionStay2D(Collision2D target)
        {
            if (target.gameObject.tag == "Boundary")
            {

                countStayBoundary += Time.deltaTime;
                //			Debug.Log ("Va cham duoc " + timeCountSameUpvector + " giay ");
                if (countStayBoundary > timeDelaySameMoveDir)
                {
                    //				Debug.Log ("Va cham duoc " + timeCountSameUpvector + " giay (lon)");
                    moveDirection *= -1;
                    countStayBoundary = 0f;
                    GameObject spawnChangeDir = Instantiate(changeDirObject);
                    spawnChangeDir.transform.position = transform.position;
                    spawnChangeDir.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

            }
            if (target.gameObject.tag == "BreakableTile")
            {

                countStayBreakable += Time.deltaTime;

                if (countStayBreakable > timeDelaySameMoveDir)
                {
                    moveDirection *= -1;
                    countStayBreakable = 0f;
                    BreakableTile breakTile = target.gameObject.GetComponent<BreakableTile>();
                    breakTile.DecreaseNumberAndCheckDestroy(newBreakDamage);
                    GameObject spawnChangeDir = Instantiate(changeDirObject);
                    spawnChangeDir.transform.position = transform.position;
                    spawnChangeDir.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

            }

        }

        private void Bounce(Vector3 collisionNormal)
        {
            var speed = lastFrameVelocity.magnitude;
            moveDirection = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
            body.linearVelocity = moveDirection * Mathf.Max(speed, moveForce);
        }

    }
}
