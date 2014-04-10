#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using System;
using System.Collections.Generic;

namespace SimpleAI.Planning
{
    public class Node : IComparable<Node>
    {
        public enum eState
        {
            kUnvisited = 0,
            kOpen,
            kClosed,
            kBlocked
        };

        #region Constants
        public const int   kInvalidIndex = -1;
        #endregion

        #region Fields
        private float       m_f;
        private float       m_g;
        private float       m_h;
        private eState      m_state;
        private Node        m_parent;
        private int         m_index;
        #endregion

        #region Properties
        public eState State
        {
            get { return m_state; }
            set { m_state = value; }
        }
        public Node Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }
        public float F
        {
            get { return m_f; }
            set { m_f = value; }
        }
        public float G
        {
            get { return m_g; }
            set { m_g = value; }
        }
        public float H
        {
            get { return m_h; }
            set { m_h = value; }
        }
        #endregion

        public Node()
        {
            Awake(kInvalidIndex, Node.eState.kUnvisited);
        }

        public int CompareTo(Node other)
        {
            if (m_f < other.m_f)
            {
                return -1;
            }
            else if (m_f > other.m_f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return "Node:" + m_f.ToString();
        }

        public void Awake(int nodeIndex, eState state)
        {
            m_index = nodeIndex;
            m_f = float.MaxValue;
            m_state = state;
            m_parent = null;
            m_g = float.MaxValue;
            m_h = float.MaxValue;
        }
    }
}
