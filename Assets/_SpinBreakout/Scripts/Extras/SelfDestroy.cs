using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class SelfDestroy : MonoBehaviour
    {

        public float timeDes = 0.8f;

        void OnEnable()
        {
            Invoke("Des", timeDes);
        }

        void Des()
        {
            Destroy(transform.gameObject);
        }
    }
}
