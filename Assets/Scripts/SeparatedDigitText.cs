using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace idbrii.game.mathmonkey
{
    public class SeparatedDigitText : MonoBehaviour
    {
        public List<TextMeshProUGUI> _Digits;

        public void SetDigit(int i, int digit_value)
        {
            var digit = _Digits[i];
            digit.text = digit_value.ToString();
        }


        public int GetValue()
        {
            var sum = 0;
            for (int i = 0; i < _Digits.Count; ++i)
            {
                var digit = _Digits[i];
                if (int.TryParse(digit.text, out int val))
                {
                    sum += val * (int)Mathf.Pow(10, i);
                }
            }
            return sum;
        }


        public void SetValue(int num)
        {
            var str = num.ToString();
            int i;
            for (i = 0; i < str.Length; ++i)
            {
                var digit = _Digits[i];
                var str_idx = str.Length - i - 1;    
                digit.text = str[str_idx].ToString();
            }
            for (; i < _Digits.Count; ++i)
            {
                var digit = _Digits[i];
                digit.text = string.Empty;
            }
        }


        public void Clear()
        {
            for (int i = 0; i < _Digits.Count; ++i)
            {
                var digit = _Digits[i];
                digit.text = string.Empty;
            }
        }

        [NaughtyAttributes.Button]
        void LabelButtonObjects()
        {
            for (int i = 0; i < _Digits.Count; ++i)
            {
                var txt = _Digits[i];
                txt.name = $"{name} - Digit {i}";
            }
        }

        [NaughtyAttributes.Button]
        void Test_GetValue()
        {
            var val = Random.Range(0,99);
            SetValue(val);
            Assert.AreEqual(val, GetValue());
            Debug.Log($"Set value to {val}", this);
        }

    }
}
