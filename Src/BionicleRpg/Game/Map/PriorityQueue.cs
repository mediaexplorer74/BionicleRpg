using GameManager.DataTypes;
using System;
using System.Collections.Generic;

namespace GameManager.Map
{
    internal class PriorityQueue<T1, T2>
    {
        //RnD
        internal int Count = 1;
        private IEnumerable<(Vector2Int, int)> enumerable;

        public PriorityQueue(IEnumerable<(Vector2Int, int)> enumerable)
        {
            this.enumerable = enumerable;
        }

        internal Vector2Int Dequeue()
        {
            //throw new NotImplementedException();
            return default;
        }

        internal void Enqueue(Vector2Int tilePos, int priority)
        {
            //throw new NotImplementedException();
        }
    }
}