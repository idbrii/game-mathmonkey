using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using TMPro;
using UnityEngine;
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

        List<TextMeshProUGUI> m_Display;

        int m_Top;
        int m_Bottom;
        Operator m_Op;
        Func<int,int,int> m_OpFunc;

        void Awake()
        {
            m_Display = GetComponentsInChildren<TextMeshProUGUI>()
                .ToList();
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

            m_Display[0].text = m_Top.ToString();
            m_Display[1].text = GetOpAsString(m_Op);
            m_Display[2].text = m_Bottom.ToString();
            m_Display[3].text = m_OpFunc(m_Top, m_Bottom).ToString();

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
