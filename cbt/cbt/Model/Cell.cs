using System;
using System.Reflection.Metadata;

namespace Sudoku.Model
{
    public class Cell
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
        public int x, y;

        public Cell(int value, int x, int y, bool set)
        {
            this.value = value;
            this.set = set;
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// Deep clone a Cell.
        /// </summary>
        public Cell Clone()
        {
            return new Cell(this.value, this.x, this.y, this.set);
        }

        public override string ToString()
        {
            return $"v{this.value} ({this.x},{this.y})";
        }
    }
}