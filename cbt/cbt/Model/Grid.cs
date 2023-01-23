using Sudoku.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Model
{
    public class Grid
    {
        /// <summary>
        /// Two-dimensional array of Cells.
        /// </summary>
        public Cell[,] tiles;

        public Grid(string prefab)
        {
            this.tiles = this.GetBaseGrid(prefab);
        }
    
        public Grid(Cell[,] tiles)
        {
            this.tiles = tiles;
        }
        
        /// <summary>
        /// Deep clone the Cells in a Grid.
        /// </summary>
        public Grid Clone()
        {
            Cell[,] newTiles = new Cell[9, 9];
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    newTiles[x, y] = this.tiles[x, y].Clone();
                }
            }

            return new Grid(newTiles);
        }

        /// <summary>
        /// Create grid from prefab input.
        /// </summary>
        private Cell[,] GetBaseGrid(string prefab)
        {
            List<int> numbers = this.Parse(prefab);
            return this.MakeGrid(numbers);
        }
        
        /// <summary>
        /// Parse the prefab string, converting it to a list of elements.
        /// </summary>
        private List<int> Parse(string prefab)
        {
            List<string> elems = prefab.Split(' ').ToList();
            return elems.Select(int.Parse).ToList();
        }
        
        /// <summary>
        /// Create the 2-D array from the list of elements.
        /// </summary>
        private Cell[,] MakeGrid(List<int> numbers)
        {
            Cell[,] newGrid = new Cell[9, 9];
            
            for(int y = 0; y < 9; y++)
            {
                for(int x = 0; x < 9; x++)
                {
                    int number = numbers[x + y * 9];
                    newGrid[x , y] = new Cell(number, x, y, number != 0);;
                }
            }
            return newGrid;
        }
        
        /// <summary>
        /// Swap the values of two Cells and save them in the Grid.
        /// Swapping is done based on the indices saved within the Cell.
        /// </summary>
        public void SwapCells((Cell, Cell) swap)
        {
            // Cells.
            Cell cellL = swap.Item1;
            Cell cellR = swap.Item2;
            
            // Swap values.
            int temp = cellL.value;
            cellL.value = cellR.value;
            cellR.value = temp;
            
            // Write new data.
            this.tiles[cellL.x, cellL.y] = cellL;
            this.tiles[cellR.x, cellR.y] = cellR;
        }

        /// <summary>
        /// Visualise the grid.
        /// </summary>
        public override string ToString()
        {
            string result = "";
            for (int y = 0; y < 9; y++)
            {
                string edgeRow = this.EdgeRow(y);
                string row = edgeRow;
                
                for (int x = 0; x < 9; x++)
                {
                    row += this.EdgeColumn(x) + this.tiles[x, y].value;
                }

                result += row + this.EdgeColumn() + "\n";
            }

            return result + this.EdgeRow();
        }

        /// <summary>
        /// Conditional vertical edge.
        /// </summary>
        private string EdgeRow(int y = 0)
        {
            if (y % 3 == 0)
            {
                return new string('-', 27) + "\n";
            }
            
            return "";
        }
        
        /// <summary>
        /// Conditional horizontal edge.
        /// </summary>
        private string EdgeColumn(int x = 0)
        {
            if (x % 3 == 0)
            {
                return " | ";
            }
            
            return " ";
        }
    }
}