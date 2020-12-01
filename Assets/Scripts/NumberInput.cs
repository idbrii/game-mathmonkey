using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using idbrii.lib.util;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace idbrii.game.mathmonkey
{
    public class NumberInput : MonoBehaviour
    {
        public GameObject _Root;
        public List<Button> _ButtonsIndexedByValue;

        Action<int> _Callback;

        IEnumerator SetActiveWithDelay(bool destination)
        {
            // Carefully tuned to display the button down tint "anim".
            yield return new WaitForSeconds(0.1f);
            _Root.SetActive(destination);
        }

        public void GetNumber(Action<int> callback)
        {
            _Callback = callback;
            GlobalCoroutine.Instance.StartCoroutine(SetActiveWithDelay(true));
        }

        public void OnPressedNumber(Button pressed)
        {
            int index = _ButtonsIndexedByValue.IndexOf(pressed);
            GlobalCoroutine.Instance.StartCoroutine(SetActiveWithDelay(false));
            _Callback(index);
        }

        public void OnPressedCancel()
        {
            GlobalCoroutine.Instance.StartCoroutine(SetActiveWithDelay(false));
            _Callback(-1);
        }

        [NaughtyAttributes.Button]
        void LabelButtonObjects()
        {
            for (int i = 0; i < _ButtonsIndexedByValue.Count; ++i)
            {
                var btn = _ButtonsIndexedByValue[i];
                btn.name = $"Button {i}";
                var txt = btn.GetComponentInChildren<TextMeshProUGUI>();
                txt.text = i.ToString();
            }
        }

    }
}
