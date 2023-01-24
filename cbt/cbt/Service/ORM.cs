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
        /// Fill each block with random numbers.
        /// </summary>
        public void FillGrid(Grid grid)
        {
            // for (int bX = 0; bX < 3; bX++)
            // {
            //     for (int bY = 0; bY < 3; bY++)
            //     {
            //         Cell[] block = this.ReadBlock(bX, bY, grid);
            //         List<int> pool = this.GenerateContenderPool(block);
            //         Cell[] blockFilled = this.FillBlock(block, pool);
            //         this.WriteBlock(bX, bY, blockFilled, grid);
            //     }
            // }
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

        public HashSet<int> GetNeighbourIndices(Grid grid, int index)
        {
            int[] rowIndices = this.GetRowIndices(index);
            int[] columnIndices = this.GetColumnIndices(index);
            int[] blockIndices = this.GetBlockIndices(grid, index);

            int[] allIndices = rowIndices.Concat(columnIndices).Concat(blockIndices).ToArray();

            HashSet<int> indexSet = new HashSet<int>(allIndices);
            indexSet.Remove(index);

            return indexSet;
        }

        public int[] GetRowIndices(int index)
        {
            int offset = index % 9;
            int startRange = index - offset;
            return Enumerable.Range(startRange, 9).ToArray();
        }

        public int[] GetColumnIndices(int index)
        {
            int[] columnIndices = new int[9];
            for (int i = 0; i < 9; i++)
            {
                columnIndices[i] = (index + i * 9) % 81;
            }
            return columnIndices;
        }

        public int[] GetBlockIndices(Grid grid, int index)
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

        public Cell[] GetCellsFromLookup(Grid grid, int[] indices)
        {
            Cell[] result = new Cell[27];

            for (int i = 0; i < 27; i++)
            {
                result[i] = grid.tiles[indices[i]];
            }

            return result;
        }

        public bool ApplyForwardChecking(Grid grid, int index, int allocatedValue)
        {
            HashSet<int> neighbourIndices = this.GetNeighbourIndices(grid, index);
            foreach (int neighbourIndex in neighbourIndices)
            {
                //delete allocatedValue
                //check if the hashset is empty
            }
            return true;
        } 
    }
}