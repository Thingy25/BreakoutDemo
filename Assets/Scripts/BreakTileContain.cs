using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class BreakTileContain : MonoBehaviour
    {

        public static BreakTileContain Instance;

        int numberOfTileChild;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void Start()
        {
            numberOfTileChild = transform.childCount;
        }

        public void UpdateNumberTileChild(int amount)
        {
            numberOfTileChild -= amount;
            if (numberOfTileChild <= 0)
            {
                //			SpawnerBreakContain.Instance.MakeBreakContain ();
                Destroy(gameObject);
                return;
            }
        }

    }
}
