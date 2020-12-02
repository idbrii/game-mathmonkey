using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
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

        public NumberInput m_Input;

        int m_Top;
        int m_Bottom;
        Operator m_Op;
        Func<int,int,int> m_OpFunc;

        void Awake()
        {
        }

        void Start()
        {
            NewPuzzle();
        }

        public void OnPressedSolution(TextMeshProUGUI solution)
        {
            m_Input.GetNumber((input) => {
                if (input <= 0)
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
            // TODO: Check if answer is right. show hints.
        }

        public void NextPuzzle()
        {
            NewPuzzle();
        }

        void NewPuzzle()
        {
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
