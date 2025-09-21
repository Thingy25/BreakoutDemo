using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public enum TurnAround
    {
        Clockwise,
        CounterClockwise,
    }

    public class CircleBreakControl : MonoBehaviour
    {
        public float angularSpeed;

        public TurnAround turnDirection;

        public PlayerController player;

        public Transform brickBreakout;

        float alphaAngel;

        bool playerIsAlive;

        float speedMultiplier = 0;

        [HideInInspector]
        public bool canClick;

        [HideInInspector]
        public bool canShootPlayer;

        int[] indexList = null;

        int countCalltime;

        //bool isShoot;

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
            PlayerController.PlayerDied += PlayerWasDie;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
            PlayerController.PlayerDied -= PlayerWasDie;
        }

        void Start()
        {
            countCalltime = 0;
            playerIsAlive = true;
            //isShoot = false;
            player.transform.SetParent(brickBreakout);
            StartCoroutine(PickDirection());
        }

        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                canShootPlayer = true;
            }
        }

        int PickRandomBackground()
        {
            if (countCalltime >= indexList.Length - 1)
            {
                countCalltime = 0;
                return countCalltime;
            }
            countCalltime++;
            return indexList[countCalltime];
        }

        void PlayerWasDie()
        {
            playerIsAlive = false;
            canClick = false;
        }

        void Update()
        {
            if (playerIsAlive)
            {
                ProcessTurnDirection();
            }
            if (playerIsAlive)
            {

                if (canShootPlayer)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        player.canMove = true;
                        player.transform.parent = null;
                        canClick = true;
                        canShootPlayer = false;
                        //isShoot = true;
                        player.ChangeMoveDirection(player.transform.position - transform.position);
                    }
                }
                speedMultiplier += Time.deltaTime;
                if (speedMultiplier >= 7)
                {
                    angularSpeed += 35;
                    speedMultiplier = 0;
                }
                //else
                //{
                //    if (isShoot)
                //    {
                //        if (Input.GetMouseButtonDown(0)) //Change to randomized Coroutine
                //        {
                //            if (canClick)
                //            {
                //                if (turnDirection == TurnAround.Clockwise)
                //                    turnDirection = TurnAround.CounterClockwise;
                //                else
                //                    turnDirection = TurnAround.Clockwise;
                //                ProcessTurnDirection();
                //                return;
                //            }
                //        }
                //    }

                //}
            }
        }

        IEnumerator PickDirection()
        {
            int newDirection;
            while (playerIsAlive)
            {
                yield return new WaitForSeconds(1.5f);
                newDirection = Random.Range(0, 2);
                Debug.Log(newDirection);

                if (newDirection == 0)
                {
                    turnDirection = TurnAround.Clockwise;
                }
                else
                {
                    turnDirection = TurnAround.CounterClockwise;
                }

            }
        }

        void ProcessTurnDirection()
        {
            switch (turnDirection)
            {
                case TurnAround.Clockwise:
                    alphaAngel += angularSpeed * Time.deltaTime;
                    transform.localEulerAngles = new Vector3(0, 0, alphaAngel);
                    break;
                case TurnAround.CounterClockwise:
                    alphaAngel -= angularSpeed * Time.deltaTime;
                    transform.localEulerAngles = new Vector3(0, 0, alphaAngel);
                    break;
                default:
                    alphaAngel += angularSpeed * Time.deltaTime;
                    transform.localEulerAngles = new Vector3(0, 0, alphaAngel);
                    return;
            }
        }

    }
}
