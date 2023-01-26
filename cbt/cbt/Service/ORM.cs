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
        /// Get set of neighbouring row indices based on given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HashSet<int> GetRowIndices(int index)
        {
            return Enumerable.Range(index - index % 9, 9).ToHashSet();
        }
        
        /// <summary>
        /// Get set of neighbouring column indices based on given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HashSet<int> GetColumnIndices(int index)
        {
            HashSet<int> result = new();
            for (int i = 0; i < 9; i++)
            {
                result.Add((index + i * 9) % 81);
            }
            return result;
        }
        
        /// <summary>
        /// Get index of upper-left corner of a block, based on given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetUpperLeftCorner(int index)
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
        
        /// <summary>
        /// Get set of neighbouring block indices based on given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HashSet<int>  GetBlockIndices(int index)
        {
            int upperLeftCorner = GetUpperLeftCorner(index);
            
            HashSet<int> result = new();
            for (int i = 0; i < 3; i++)
            {
                HashSet<int> rowIndices = Enumerable.Range(upperLeftCorner + 9 * i, 3).ToHashSet();
                result.UnionWith(rowIndices);
            }
            return result;
        }
        
        /// <summary>
        /// Get set of neighbouring row-, column- and block indices based on given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HashSet<int> GetNeighbourIndices(int index)
        {
            HashSet<int> result = this.GetBlockIndices(index);
            result.UnionWith(this.GetRowIndices(index));
            result.UnionWith(this.GetColumnIndices(index));
            result.Remove(index);
            return result;
        }
        
        /// <summary>
        /// Make cell on given neighbour index consistent by reducing its domain.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="neighbourIndex"></param>
        /// <param name="sourceCell"></param>
        /// <returns></returns>
        private bool ReduceSingleDomain(Grid grid, int neighbourIndex, Cell sourceCell)
        {
            Cell cell = grid.GetCell(neighbourIndex);
            return cell.ApplyDomainConsistency(sourceCell.value);
        }
        
        /// <summary>
        /// Make constraints reliant on given index consistent. Early stop if empty domain is created in said way.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ApplyForwardChecking(Grid grid, int index)
        {
            Cell sourceCell = grid.GetCell(index);
            
            HashSet<int> neighbourIndices = this.GetNeighbourIndices(index);
            foreach (int neighbourIndex in neighbourIndices)
            {
                if (!this.ReduceSingleDomain(grid, neighbourIndex, sourceCell))
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Make all domains consistent.
        /// </summary>
        /// <param name="grid"></param>
        public void SetupDomains(Grid grid)
        {
            for (int i = 0; i < 81; i++)
            {
                this.ApplyForwardChecking(grid, i);
            }
        }
    }
}