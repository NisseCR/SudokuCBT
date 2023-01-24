using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sudoku.Service;

namespace Sudoku.Model
{
    public class State
    {
        /// <summary>
        /// The Sudoku puzzle.
        /// </summary>
        public Grid grid;

        /// <summary>
        /// Normal constructor.
        /// </summary>
        public State(Grid grid)
        {
            this.grid = grid;
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