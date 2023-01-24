using System;
using System.Collections.Generic;
using System.Linq;
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

        public int[] GetNeighbourIndices(Grid grid, int i)
        {
            // i = 0
            // collection = 0..8
            
            return null;
        }

        public void ApplyForwardChecking(Grid grid, int[] indices, int allocatedValue)
        {
            return;
        }
    }
}