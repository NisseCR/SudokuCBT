using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sudoku.Model;

namespace Sudoku.Service
{
    public class ORM
    {
        /// <summary>
        /// Get the contents of the specified block.
        /// </summary>
        public Cell[] ReadBlock(int bX, int bY, Grid grid)
        {
            Cell[] block = new Cell[9];

            int i = 0;
            for (int x = bX * 3; x < (bX + 1) * 3; x++)
            {
                for (int y = bY * 3; y < (bY + 1) * 3; y++)
                {
                    block[i++] = grid.tiles[x, y];
                }
            }
            
            return block;
        }
        
        /// <summary>
        /// Write content to the specified block.
        /// </summary>
        public Grid WriteBlock(int bX, int bY, Cell[] block, Grid grid)
        {
            int i = 0;
            for (int x = bX * 3; x < (bX + 1) * 3; x++)
            {
                for (int y = bY * 3; y < (bY + 1) * 3; y++)
                {
                    grid.tiles[x, y] = block[i++];
                }
            }
            
            return grid;
        }
        
        /// <summary>
        /// Fill each block with random numbers.
        /// </summary>
        public void FillGrid(Grid grid)
        {
            for (int bX = 0; bX < 3; bX++)
            {
                for (int bY = 0; bY < 3; bY++)
                {
                    Cell[] block = this.ReadBlock(bX, bY, grid);
                    List<int> pool = this.GenerateContenderPool(block);
                    Cell[] blockFilled = this.FillBlock(block, pool);
                    this.WriteBlock(bX, bY, blockFilled, grid);
                }
            }
        }
        
        /// <summary>
        /// Fill non-fixed cells within the block with random numbers, such that the block contains numbers {1..9}.
        /// </summary>
        private Cell[] FillBlock(Cell[] block, List<int> pool)
        {
            Cell[] blockFilled = new Cell[9];

            for (int i = 0; i < block.Length; i++)
            {
                Cell cell = block[i];

                if (!cell.set)
                {
                    cell.value = pool.First();
                    pool.RemoveAt(0);
                }

                blockFilled[i] = cell;
            }

            return blockFilled;
        }
        
        /// <summary>
        /// Create a pool of numbers that are to be randomly inserted into a block.
        /// </summary>
        private List<int> GenerateContenderPool(Cell[] block)
        {
            List<int> pool = Enumerable.Range(1, 9).ToList();
            foreach (Cell cell in block)
            {
                if (!cell.set)
                {
                    continue;
                }

                pool.Remove(cell.value);
            }

            Random rand = new Random();
            return pool.OrderBy(_ => rand.Next()).ToList();
        }
        
        /// <summary>
        /// Get a list of all possible swaps between cells in a block.
        /// Each cell stores its index relative to the grid, two cells contain enough information to create a successor.
        /// Permutation order is random, hence selecting a successor is un-biased.
        /// </summary>
        public List<(Cell, Cell)> GetSwaps(Cell[] block)
        {
            block = this.FilterFixed(block);
            return this.Permutations(block);
        }
        
        /// <summary>
        /// Filter all set Cells, allowing swaps to be made only on non-fixed Cells.
        /// </summary>
        public Cell[] FilterFixed(Cell[] block)
        {
            return block.Where(i => !i.set).ToArray();
        }
        
        /// <summary>
        /// Generate permutations of all swap combinations.
        /// </summary>
        private List<(Cell, Cell)> Permutations(Cell[] block)
        {
            List<(Cell, Cell)> swaps = new List<(Cell, Cell)>();
            
            for (int i = 0; i < block.Length - 1; i++)
            {
                Cell cellL = block[i];
                for (int j = i + 1; j < block.Length; j++)
                {
                    Cell cellR = block[j];
                    swaps.Add((cellL.Clone(), cellR.Clone()));
                }
            }
            
            // Unbiased swap list.
            Random rand = new Random();
            return swaps.OrderBy(_ => rand.Next()).ToList();
        }
        
        /// <summary>
        /// Get a lis of all rows, used for heuristic value calculation.
        /// </summary>
        public List<Cell[]> GetRows(Grid grid)
        {
            List<Cell[]> rows = new();
            for (int i = 0; i < 9; i++)
            {
                rows.Add(this.GetRow(grid, i));
            }

            return rows;
        }
        
        /// <summary>
        /// Get a lis of all columns, used for heuristic value calculation.
        /// </summary>
        public List<Cell[]> GetColumns(Grid grid)
        {
            List<Cell[]> columns = new();
            for (int i = 0; i < 9; i++)
            {
                columns.Add(this.GetColumn(grid, i));
            }

            return columns;
        }

        /// <summary>
        /// source: https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp
        /// </summary>
        public Cell[] GetRow(Grid grid, int rowNumber)
        {
            return Enumerable.Range(0, grid.tiles.GetLength(0))
                .Select(x => grid.tiles[x, rowNumber])
                .ToArray();
        }
        
        /// <summary>
        /// source: https://stackoverflow.com/questions/27427527/how-to-get-a-complete-row-or-column-from-2d-array-in-c-sharp
        /// </summary>
        public Cell[] GetColumn(Grid grid, int columnNumber)
        {
            return Enumerable.Range(0, grid.tiles.GetLength(1))
                .Select(x => grid.tiles[columnNumber, x])
                .ToArray();
        }

        public HashSet<int> GetNeighbourIndices(Grid grid, int index)
        {
            int[] rowIndices = this.GetRowIndices(grid, index);
            int[] columnIndices = this.GetColumnIndices(grid, index);
            int[] blockIndices = this.GetBlockIndices(grid, index);

            int[] allIndices = rowIndices.Concat(columnIndices).Concat(blockIndices).ToArray();

            HashSet<int> indexSet = new HashSet<int>(allIndices);
            indexSet.Remove(index);

            return indexSet;
        }

        private int[] GetRowIndices(Grid grid, int index)
        {
            int offset = index % 9;
            int startRange = index - offset;
            return Enumerable.Range(startRange, 9).ToArray();
        }

        private int[] GetColumnIndices(Grid grid, int index)
        {
            int[] columnIndices = new int[8];
            for (int i = 1; i < 9; i++)
            {
                columnIndices[i - 1] = (index + i * 9) % 81;
            }
            return columnIndices;
        }

        private int[] GetBlockIndices(Grid grid, int index)
        {
            int upperLeftCorner = GetUpperLeftCorner(grid, index);
            List<int> blockIndices = new();
            for (int i = 0; i < 3; i++)
            {
                List<int> indicesRow = Enumerable.Range(upperLeftCorner + 9 * i, 3).ToList();
                blockIndices.AddRange(indicesRow);
            }
            return blockIndices.ToArray();
        }

        private int GetUpperLeftCorner(Grid grid, int index)
        {
            int offsetRight = index % 3;
            int leftIndex = index - offsetRight;
            int height = leftIndex - (leftIndex % 9);

            int offsetDown = 0;
            while (height % 27 != 0)
            {
                height -= 9;
                offsetDown++;
            }
            return leftIndex - offsetDown * 9;
        }

        public void ApplyForwardChecking(Grid grid, int[] indices, int allocatedValue)
        {
            return;
        }
    }
}