using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using idbrii.lib.inspector;
using idbrii.lib.util;

using Random = UnityEngine.Random;

namespace idbrii.game.mathmonkey
{
    public class Logic : MonoBehaviour
    {

        enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        public SeparatedDigitText m_TopText;
        public SeparatedDigitText m_BottomText;
        public TextMeshProUGUI m_OpText;
        public SeparatedDigitText m_SolutionDigits;

        List<Image> m_SolutionButtons;

        public Color _Correct = Color.green;
        public Color _Neutral = Color.yellow;
        public Color _Incorrect = Color.red;

        public NumberInput m_Input;

        bool _CanChangeValues = true;
        int m_Top;
        int m_Bottom;
        Operator m_Op;
        Func<int,int,int> m_OpFunc;

        void Awake()
        {
        }

        void Start()
        {
            m_SolutionButtons = m_SolutionDigits._Digits
                .Select(d => d.GetComponentInParent<Image>())
                .ToList();

            NewPuzzle();
        }

        public void OnPressedSolution(TextMeshProUGUI solution)
        {
            if (!_CanChangeValues)
            {
                // TODO: error flash/sound
                return;
            }

            m_Input.GetNumber((input) => {
                if (input < 0)
                {
                    // User cancelled
                    return;
                }
                solution.text = input.ToString();
                ProcessSolution();
            });
        }

        void ProcessSolution()
        {
            var guess = m_SolutionDigits.GetValue();
            var correct = m_OpFunc(m_Top, m_Bottom);
            if (guess == correct)
            {
                foreach (var img in m_SolutionButtons)
                {
                    img.color = _Correct;
                    StartCoroutine(TriggerPuzzleCompetion());
                }
            }
            else
            {
                var guess_str = guess.ToString();
                var correct_str = correct.ToString();

                //~ Debug.Log($"Total guess {guess}, correct {correct}", this);
                int i;
                for (i = 0; i < guess_str.Length; ++i)
                {
                    var guess_digit = guess_str[guess_str.Length - i - 1];
                    var correct_digit = correct_str[correct_str.Length - i - 1];
                    var c = _Incorrect;
                    if (guess_digit == correct_digit)
                    {
                        c = _Correct;
                    }
                    //~ Debug.Log($"Digit[{i}] guess {guess_digit}, correct {correct_digit}", this);
                    m_SolutionButtons[i].color = c;
                }
                for (; i < m_SolutionButtons.Count; ++i)
                {
                    m_SolutionButtons[i].color = _Neutral;
                }
            }
        }

        IEnumerator TriggerPuzzleCompetion()
        {
            _CanChangeValues = false;

            // TODO: celebration
            yield return new WaitForSeconds(5f);

            NewPuzzle();
            _CanChangeValues = true;
        }

        public void NextPuzzle()
        {
            NewPuzzle();
        }

        void NewPuzzle()
        {
            foreach (var img in m_SolutionButtons)
            {
                img.color = _Neutral;
            }

            var max_op = EnumTool.GetLength<Operator>();
            // TODO: Difficulty levels 
            // harder means:
            // * higher op
            // * higher values
            max_op = 0;
            m_Top = Random.Range(0, 20);
            m_Bottom = Random.Range(0, 20);
            m_Op = (Operator)Random.Range(0, max_op);
            m_OpFunc = GetOpAsFunc(m_Op);

            m_TopText.SetValue(m_Top);
            m_BottomText.SetValue(m_Bottom);
            m_OpText.text = GetOpAsString(m_Op);

            m_SolutionDigits.Clear();
        }

        void FillInSolution()
        {
            m_SolutionDigits.SetValue(m_OpFunc(m_Top, m_Bottom));
        }

        string GetOpAsString(Operator op)
        {
            switch (op)
            {
                case Operator.Add:
                    return "+";
                    
                case Operator.Subtract:
                    return "-";
                    
                case Operator.Multiply:
                    return "×";
                    
                case Operator.Divide:
                    return "÷";
            }
            Debug.Assert(false, $"Failed to handle op {op}");
            return "??";
        }

        Func<int,int,int> GetOpAsFunc(Operator op)
        {
            switch (op)
            {
                case Operator.Add:
                    return (a,b) => a + b;
                    
                case Operator.Subtract:
                    return (a,b) => a - b;
                    
                case Operator.Multiply:
                    return (a,b) => a * b;
                    
                case Operator.Divide:
                    return (a,b) => a / b;
            }
            Debug.Assert(false, $"Failed to handle op {op}");
            return null;
        }

    }
}
