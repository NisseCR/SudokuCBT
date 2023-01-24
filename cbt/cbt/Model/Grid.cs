using Sudoku.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Model
{
    public struct Grid : ICloneable
    {
        /// <summary>
        /// Two-dimensional array of Cells.
        /// </summary>
        public Cell[] tiles;

        public Grid(string prefab)
        {
            this.tiles = Parse(prefab);
        }

        public Grid(Cell[] source)
        {
            Cell[] newTiles = new Cell[81];
            for (int i = 0; i < 81; i++)
            {
                newTiles[i] = (Cell) source[i].Clone();
            }

            this.tiles = newTiles;
        }

        public Object Clone()
        {
            return new Grid(this.tiles);
        }
        

        /// <summary>
        /// Create grid from prefab input.
        /// </summary>
        private static Cell[] Parse(string prefab)
        {
            string[] elems = prefab.Split(' ').ToArray();
            return elems.Select(CreateCell).ToArray();
        }

        private static Cell CreateCell(string item, int index)
        {
            int number = int.Parse(item);
            return new Cell(number, index, number != 0);
        }

        /// <summary>
        /// Visualise the grid.
        /// </summary>
        public override string ToString()
        {
            string result = "";
            foreach (Cell cell in this.tiles)
            {
                result += $"{cell.value} ";
            }

            return result;
        }
    }
}