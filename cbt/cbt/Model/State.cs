using System;

namespace Sudoku.Model
{
    public struct State : ICloneable
    {
        /// <summary>
        /// The Sudoku puzzle.
        /// </summary>
        public Grid grid;

        public int successorValue;
        public bool noSuccessors;
        public int step;

        public State(Grid grid, int? successorValue = null, bool? noSuccessors = false)
        {
            this.grid = grid;
            this.successorValue = successorValue ?? 0;
            this.noSuccessors = noSuccessors ?? false;
            this.step = 1;
        }
        
        public object Clone()
        {
            return new State((Grid) grid.Clone());
        }
        
        /// <summary>
        /// Dynamically look for next successor, if there are any left.
        /// </summary>
        /// <param name="cell"></param>
        public void FindNextSuccessor(Cell cell)
        {
            if (this.successorValue > 9)
            {
                this.noSuccessors = true;
                return;
            }

            if (!cell.IsInDomain(this.successorValue))
            {
                this.successorValue++;
                this.FindNextSuccessor(cell);
            }
        }
        
        /// <summary>
        /// Wrapper method.
        /// </summary>
        /// <param name="cell"></param>
        public void GetNextSuccessorValue(Cell cell)
        {
            this.successorValue++;
            this.FindNextSuccessor(cell);
        }

        /// <summary>
        /// Visualise the state.
        /// </summary>
        public override string ToString()
        {
            
            return $"Next successorValue: ({this.successorValue})" + this.grid.ToString();
        }
    }
}