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

        enum MathComponents
        {
            Top,
            Bottom,
            Op,
        }

        [EnumArray(typeof(MathComponents))]
        public TextMeshProUGUI[] m_Display = new TextMeshProUGUI[EnumTool.GetLength<MathComponents>()];

        public TextMeshProUGUI[] m_SolutionDigits;

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

            m_Display[(int)MathComponents.Top].text = m_Top.ToString();
            m_Display[(int)MathComponents.Op].text = GetOpAsString(m_Op);
            m_Display[(int)MathComponents.Bottom].text = m_Bottom.ToString();
            var solution = m_OpFunc(m_Top, m_Bottom).ToString();
            int i;
            for (i = 0; i < solution.Length; ++i)
            {
                var digit = m_SolutionDigits[i];
                var str_idx = solution.Length - i - 1;    
                digit.text = solution[str_idx].ToString();
            }
            for (; i < m_SolutionDigits.Length; ++i)
            {
                var digit = m_SolutionDigits[i];
                digit.text = string.Empty;
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
                    return (a,b) => a / b;
            }
            Debug.Assert(false, $"Failed to handle op {op}");
            return null;
        }

    }
}
