using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleBreak
{
    public class ResizeBoundary : MonoBehaviour
    {

        EdgeCollider2D edgeCollider;
        LineRenderer lineRender;

        [SerializeField]
        protected Transform cornerTR, cornerTL, cornerBL, cornerBR;

        Vector3 topRight;
        Vector3 topLeft;
        Vector3 botLeft;
        Vector3 botRight;

        void Awake()
        {
            StartCoroutine(CR_SetUPBoundary());
        }

        IEnumerator CR_SetUPBoundary()
        {
            yield return new WaitForSeconds(0.1f);
            edgeCollider = GetComponent<EdgeCollider2D>();
            topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
            botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            botRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            botLeft.y += 1.7f;
            botRight.y += 1.7f;
            topRight.z = topLeft.z = botLeft.z = botRight.z = 0;

            cornerTR.position = topRight;
            cornerTL.position = topLeft;
            cornerBL.position = botLeft;
            cornerBR.position = botRight;

            lineRender = GetComponent<LineRenderer>();
            lineRender.positionCount = 5;
            Vector3[] borderList = new Vector3[5] { topRight, topLeft, botLeft, botRight, topRight };
            lineRender.SetPositions(borderList);

            Vector2[] colliderPoints = edgeCollider.points;

            colliderPoints[0] = new Vector2(topRight.x, topRight.y);
            colliderPoints[edgeCollider.points.Length - 1] = new Vector2(topRight.x, topRight.y);
            colliderPoints[1] = new Vector2(topLeft.x, topLeft.y);
            colliderPoints[2] = new Vector2(botLeft.x, botLeft.y);
            colliderPoints[3] = new Vector2(botRight.x, botRight.y);
            edgeCollider.points = colliderPoints;
        }

        public void ResizeFollowScreenSize()
        {
            float aspect = (float)Screen.height / (float)Screen.width;
            float orthorgraphicSize = Camera.main.orthographicSize;
            if (aspect >= 0.62f && aspect <= 0.63f)
            {
                //10:16

            }
            else if (aspect >= 0.56f && aspect <= 0.57f)
            {
                //9:16   1242:2208

            }
            else if (aspect >= 0.65f && aspect <= 0.67f)
            {
                //2:3  800:1200

            }
            else if (aspect >= 0.74f && aspect <= 0.76f)
            {
                //3:4  2048:2732

            }
            else if (aspect >= 0.59f && aspect <= 0.61f)
            {
                //3:5

            }
            else if (aspect >= 0.79f && aspect <= 0.81f)
            {
                //4:5

            }
        }
    }
}
