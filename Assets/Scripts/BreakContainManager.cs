using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class BreakContainManager : MonoBehaviour
    {

        public static BreakContainManager Instance;

        [Header("Break contain list")]
        public GameObject[] breakContainList;

        int[] arrayIndex;

        int countCallTime;

        void Awake()
        {
            if (Instance != null)
                DestroyImmediate(gameObject);
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            arrayIndex = Utilities.GenerateShuffleIndices(breakContainList.Length);
        }

        void Start()
        {
            countCallTime = 0;
        }

        public int GetIndexAfterStuffleList()
        {
            if (countCallTime >= arrayIndex.Length - 1)
            {
                countCallTime = 0;
                arrayIndex = Utilities.GenerateShuffleIndices(breakContainList.Length);
                return countCallTime;
            }
            countCallTime++;
            return arrayIndex[countCallTime];
        }

    }
}
