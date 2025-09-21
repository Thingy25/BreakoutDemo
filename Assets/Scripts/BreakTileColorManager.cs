using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class BreakTileColorManager : MonoBehaviour
    {

        public static BreakTileColorManager Instance;

        [Header("The color list follow number of breakable tile")]
        public Color[] tileColorList;

        void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
