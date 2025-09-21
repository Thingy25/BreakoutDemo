using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class SpawnerBreakContain : MonoBehaviour
    {

        public static SpawnerBreakContain Instance;

        [Header("Object's transform reference")]
        public Transform topSpawnerStart;
        public Transform topSpawnerEnd;
        public Transform botSpawnerStart;
        public Transform botSpawnerEnd;

        [Header("Ease type of move break contain")]
        public AnimationCurve easeType;

        public float lerpDurationTime = 1f;

        bool canSpawner;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void OnEnable()
        {
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= OnGameStateChanged;
        }

        void OnGameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.Playing)
            {
                canSpawner = true;
            }
        }

        void Update()
        {
            if (canSpawner)
                MakeBreakContain();
        }

        public void MakeBreakContain()
        {
            if (topSpawnerStart.childCount <= 1)
            {
                GameObject newBreakContain = Instantiate(BreakContainManager.Instance.breakContainList[BreakContainManager.Instance.GetIndexAfterStuffleList()]);
                //			GameObject newBreakContain=Instantiate(BreakContainManager.Instance.breakContainList[0]);
                newBreakContain.transform.SetParent(topSpawnerStart.transform);
                newBreakContain.transform.localPosition = Vector3.zero;
                StartCoroutine(CrMoveBreakContain(newBreakContain, topSpawnerEnd.localPosition));
            }
            if (botSpawnerStart.childCount <= 1)
            {
                GameObject newBreakContain = Instantiate(BreakContainManager.Instance.breakContainList[BreakContainManager.Instance.GetIndexAfterStuffleList()]);
                //			GameObject newBreakContain=Instantiate(BreakContainManager.Instance.breakContainList[0]);
                newBreakContain.transform.SetParent(botSpawnerStart.transform);
                newBreakContain.transform.localPosition = Vector3.zero;
                newBreakContain.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                for (int i = 0; i < newBreakContain.transform.childCount; i++)
                {
                    newBreakContain.transform.GetChild(i).transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                }
                StartCoroutine(CrMoveBreakContain(newBreakContain, botSpawnerEnd.localPosition));
            }
        }

        IEnumerator CrMoveBreakContain(GameObject itemNeedMove, Vector3 targetPos)
        {
            float currentTime = 0;
            Vector3 pos = itemNeedMove.transform.localPosition;
            while (currentTime <= lerpDurationTime)
            {
                currentTime += Time.deltaTime;
                float t = Mathf.Clamp01(currentTime / lerpDurationTime);
                itemNeedMove.transform.localPosition = Vector3.Lerp(pos, targetPos, easeType.Evaluate(t));
                yield return null;
            }
        }

    }
}
