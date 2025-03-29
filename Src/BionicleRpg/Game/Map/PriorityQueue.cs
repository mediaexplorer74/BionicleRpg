using GameManager.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager.Map
{
    internal class PriorityQueue<T1, T2>
    {
        internal int Count = 0;
        private IEnumerable<(Vector2Int, int)> enumerable;

        public PriorityQueue(IEnumerable<(Vector2Int, int)> enumerable)
        {
            this.enumerable = enumerable;
        }

        internal Vector2Int Dequeue()
        {
            if (enumerable == null || !enumerable.Any())
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            var minElement = enumerable.OrderBy(e => e.Item2).First();
            enumerable = enumerable.Where(e => !e.Equals(minElement));
            Count--;

            return minElement.Item1;
        }

        internal void Enqueue(Vector2Int tilePos, int priority)
        {
            if (enumerable == null)
            {
                enumerable = new List<(Vector2Int, int)>();
            }

            var list = enumerable.ToList();
            list.Add((tilePos, priority));
            enumerable = list;
            Count++;
        }
    }
}