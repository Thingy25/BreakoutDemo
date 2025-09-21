using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CircleBreak
{
    [System.Serializable]
    public class RangeNumberRandom
    {
        public int minRandom;
        public int maxRandom;
    }


    public class BreakableTile : MonoBehaviour
    {

        public static BreakableTile Instance;

        [Header("Decide to random number or control by hand")]
        public bool isRandomNumber = true;

        [Header("Control by hand")]
        public int setNumberByHand;

        [Header("Max range number random automatically")]
        readonly int maxMultiplier = 5;
        public int maxRangeNumber = 5;
        public float maxMultiplierValue = 2.5f;
        [SerializeField]
        float currentMultiplier;

        public int numberToBreak;

        Text numberText;

        SpriteRenderer skin;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            skin = GetComponent<SpriteRenderer>();
            numberText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();           
        }

        void Start()
        {
            maxRangeNumber = maxMultiplier;
            if (isRandomNumber)
            {
                if (maxRangeNumber == 1)
                {
                    numberToBreak = 1;
                    numberText.text = "x " + numberToBreak.ToString();
                    UpdateColorByNumber(numberToBreak);
                    return;
                }
                else {
                    currentMultiplier = Random.Range(0, maxMultiplierValue);
                    currentMultiplier = RoundValue(currentMultiplier);
                    numberToBreak = Random.Range(1, maxRangeNumber + 1);
                    if (numberToBreak > BreakTileColorManager.Instance.tileColorList.Length - 1)
                    {
                        numberToBreak = BreakTileColorManager.Instance.tileColorList.Length - 1;
                    }
                    numberText.text = "x " + currentMultiplier.ToString();
                    UpdateColorByNumber(numberToBreak);
                    return;
                }

            }
            else {
                numberToBreak = setNumberByHand;
                if (numberToBreak > BreakTileColorManager.Instance.tileColorList.Length - 1)
                {
                    numberToBreak = BreakTileColorManager.Instance.tileColorList.Length - 1;
                }
                numberText.text = "x " + numberToBreak.ToString();
                UpdateColorByNumber(numberToBreak);
            }
        }

        void OnValidate()
        {
            if (setNumberByHand <= 0)
                setNumberByHand = 1;
            if (maxRangeNumber <= 0)
                maxRangeNumber = 1;
        }

        public void DecreaseNumberAndCheckDestroy(float amount)//HERE
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.coin);
            ScoreManager.Instance.SetNewMultiplier(currentMultiplier);
            currentMultiplier -= amount;
            currentMultiplier = RoundValue(currentMultiplier);
            if (currentMultiplier < 0)
            {
                Destroy(gameObject);
                GetComponentInParent<BreakTileContain>().UpdateNumberTileChild(1);
                return;
            }
            numberText.text = "x " + currentMultiplier.ToString();
            UpdateColorByNumber(numberToBreak);
        }

        void UpdateColorByNumber(int newNumber)
        {
            skin.color = BreakTileColorManager.Instance.tileColorList[newNumber - 1];
        }

        float RoundValue (float valueToRound)
        {
            float roundedValue = Mathf.Round(valueToRound * 10) / 10;
            return roundedValue;
        }
    }
}
