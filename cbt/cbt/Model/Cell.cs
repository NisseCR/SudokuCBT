using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Sudoku.Model
{
    public struct Cell : ICloneable
    {
        /// <summary>
        /// The actual number stored in the sudoku puzzle.
        /// </summary>
        public int value;
        
        /// <summary>
        /// Indicator that specifies whether the cell is fixed.
        /// </summary>
        public bool set;

        /// <summary>
        /// Cell saves their index relative to the grid.
        /// </summary>
        public int index;

        /// <summary>
        /// Domain of variable.
        /// </summary>
        public HashSet<int> domain;

        public Cell(int value, int index, bool set, HashSet<int> domain = null)
        {
            this.value = value;
            this.set = set;
            this.index = index;
            this.domain = domain ?? Enumerable.Range(1, 9).ToHashSet();
        }
        
        /// <summary>
        /// Deep clone a Cell.
        /// </summary>
        public Object Clone()
        {
            return new Cell(this.value, this.index, this.set, this.domain);
        }

        public override string ToString()
        {
            return $"v{this.value} i{this.index}";
        }
    }
}