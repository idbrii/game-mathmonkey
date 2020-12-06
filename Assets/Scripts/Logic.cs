using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;
using idbrii.MoreMath;
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
        public Transform _Victory;
        List<Image> _VictoryInitialConfetti;
        List<Vector3> _VictoryInitialPositions;

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

            _VictoryInitialConfetti = _Victory.GetComponentsInChildren<Image>()
                .ToList();
            _VictoryInitialPositions = _VictoryInitialConfetti
                .Select(s => s.transform.position)
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
            if (guess < 0)
            {
                // Invalid input. Probably haven't completed.
                foreach (var img in m_SolutionButtons)
                {
                    img.color = _Neutral;
                }
            }
            else if (guess == correct)
            {
                foreach (var img in m_SolutionButtons)
                {
                    img.color = _Correct;
                }
                StartCoroutine(TriggerPuzzleCompetion());
            }
            else
            {
                var guess_str = guess.ToString();
                var correct_str = correct.ToString($"D{m_SolutionButtons.Count}");

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
                {
                    var c = _Incorrect;
                    for (; i < m_SolutionButtons.Count; ++i)
                    {
                        var correct_digit = correct_str[correct_str.Length - i - 1];
                        //~ Debug.Log($"Digit[{i}] guess is blank, correct {correct_digit}", this);
                        if (correct_digit == '0')
                        {
                            c = _Neutral;
                        }
                        else
                        {
                            c = _Incorrect;
                        }
                        m_SolutionButtons[i].color = c;
                    }
                }
            }
        }

        [Range(1f,200f)]
        public float _Victory_AnimUp = 90f;
        [Range(0.1f,9f)]
        public float _Victory_AnimSpeed = 4f;
        [Range(1f,270f)]
        public float _Victory_AnimRotateSpeed = 10f;
        [Range(1f,10f)]
        public float _Victory_AnimSeconds = 5f;
        IEnumerator TriggerPuzzleCompetion()
        {
            _CanChangeValues = false;

            yield return new WaitForSeconds(1f);
            _Victory.gameObject.SetActive(true);

            var start_time = Time.time;
            var elapsed = 0f;
            while (elapsed < _Victory_AnimSeconds)
            {
                elapsed = Time.time - start_time;
                for (int i = 0; i < _VictoryInitialConfetti.Count; ++i)
                {
                    var t = _VictoryInitialConfetti[i].transform;
                    var pos = t.position;
                    pos = _VictoryInitialPositions[i] + Vector3.up * Mathf.Sin(_Victory_AnimSpeed * (elapsed + t.position.x)) * _Victory_AnimUp;
                    t.position = pos;

                    var rot = t.rotation.eulerAngles;
                    rot.z = Mathf.MoveTowardsAngle(rot.z, rot.z + _Victory_AnimRotateSpeed, 360f);
                    t.rotation = Quaternion.Euler(rot);
                }
                yield return null;
            }

            NewPuzzle();
            _CanChangeValues = true;
        }

        public void NextPuzzle()
        {
            NewPuzzle();
        }

        void NewPuzzle()
        {
            _Victory.gameObject.SetActive(false);
            foreach (var img in m_SolutionButtons)
            {
                img.color = _Neutral;
            }

            var max_op = EnumTool.GetLength<Operator>();
            // TODO: Difficulty levels 
            // harder means:
            // * higher op
            // * higher values
            m_Top = Random.Range(0, 20);
            m_Bottom = Random.Range(0, 20);
            m_Op = (Operator)Random.Range(0, max_op + 1);
            m_OpFunc = GetOpAsFunc(m_Op);

            switch (m_Op)
            {
                case Operator.Add:
                    break;
                    
                case Operator.Subtract:
                    // No support for negative answers, so ensure nonnegative.
                    SelectBigger(ref m_Top, ref m_Bottom);
                    break;
                    
                case Operator.Multiply:
                    // Only simple multiplication.
                    m_Top = Random.Range(0, 10);
                    m_Bottom = Random.Range(0, 10);
                    break;
                    
                case Operator.Divide:
                    // Ensure whole numbers
                    m_Bottom = Random.Range(1, 9);
                    m_Top = m_Bottom * Random.Range(0, 9);
                    break;
            }

            ValidateSolutionFits(m_OpFunc(m_Top, m_Bottom));

            m_TopText.SetValue(m_Top);
            m_BottomText.SetValue(m_Bottom);
            m_OpText.text = GetOpAsString(m_Op);

            m_SolutionDigits.Clear();
        }

        void ValidateSolutionFits(int solution)
        {
            Assert.IsTrue(solution >= 0);
            Assert.IsTrue(solution < 999);
        }

        void SelectBigger(ref int select_bigger, ref int select_smaller)
        {
            var big = Mathf.Max(select_bigger, select_smaller);
            var small = Mathf.Min(select_bigger, select_smaller);
            select_bigger = big;
            select_smaller = small;
        }

        [NaughtyAttributes.Button]
        void FillInSolution()
        {
            m_SolutionDigits.SetValue(m_OpFunc(m_Top, m_Bottom));
            if (Application.isPlaying)
            {
                ProcessSolution();
            }
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
                    return (a,b) => {
                        var div = Math.DivRem(a, b, out int rem);
                        Debug.Assert(rem == 0, $"Impossible divide: {a}/{b} is not a whole number. Remainder {rem}.");
                        return div;
                    };
            }
            Debug.Assert(false, $"Failed to handle op {op}");
            return null;
        }

    }
}
