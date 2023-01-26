using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sudoku.Service;

namespace Sudoku.Model
{
    public struct State : ICloneable
    {
        /// <summary>
        /// The Sudoku puzzle.
        /// </summary>
        public Grid grid;

        public int domainIndex;
        
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Normal constructor.
        /// </summary>
        public State(Grid grid)
        {
            this.grid = grid;
            this.domainIndex = 1;
        }

        /// <summary>
        /// Visualise the state.
        /// </summary>
        public override string ToString()
        {
            return this.grid.ToString();
        }
    }
}