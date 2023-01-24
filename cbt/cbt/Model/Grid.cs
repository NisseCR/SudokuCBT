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
            // Setup.
            string result = this.BlockRowEdge(26) + this.GridEdge(26);
            
            // Create boxes in representation.
            for (int i = 0; i < 81; i++)
            {
                result += $"{this.tiles[i].value}" + this.BlockColumEdge(i) + this.BlockRowEdge(i) + this.GridEdge(i);
            }
            
            return result.Remove(result.Length - 2, 2); 
        }

        private string GridEdge(int i)
        {
            return (i + 1) % 9 == 0 ? "\n| " : "";
        }

        private string BlockColumEdge(int i)
        {
            return (i + 1) % 3 == 0 ? " | " : " ";
        }

        private string BlockRowEdge(int i)
        {
            string delimiter = new string('-', 25);
            return (i + 1) % 27 == 0 ? $"\n{delimiter}" : "";
        }
    }
}