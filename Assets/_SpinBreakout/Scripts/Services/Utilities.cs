using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

namespace CircleBreak
{
    public static class Utilities
    {
        //public static IEnumerator CRWaitForRealSeconds(float time)
        //{
        //    float start = Time.realtimeSinceStartup;

        //    while (Time.realtimeSinceStartup < start + time)
        //    {
        //        yield return null;
        //    }
        //}

        public static void ButtonClickSound()
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        }

        // Opens a specific scene
        public static void GoToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static int[] GenerateShuffleIndices(int length)
        {
            int[] array = new int[length];

            // Populate array
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            // Shuffle
            for (int j = 0; j < array.Length; j++)
            {
                int tmp = array[j];
                int randomPos = UnityEngine.Random.Range(j, array.Length);
                array[j] = array[randomPos];
                array[randomPos] = tmp;
            }

            return array;
        }
    }
}