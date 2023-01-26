using System;
using System.Collections;
using System.Collections.Generic;
using Sudoku.Model;

namespace Sudoku.Service
{
    public class CBT
    {

        /// <summary>
        /// Perform DFS with Backtracking & Forward checking on a incomplete Sudoku puzzle
        /// </summary>
        /// <param name="grid">The incomplete Sudoku puzzle</param>
        public void Run(Grid grid, ORM orm)
        {

            // Initialize Stack
            Stack front = new Stack();
            front.Push(grid);

            Grid currentGrid;
            int index = 0;
            while (front.Count > 0 && index < 81)
            {
                // Get DFS successor.
                currentGrid = (Grid) front.Pop();
                Grid? nextGrid = this.GetSuccessor(currentGrid, index);
                
                // Debug
                Console.WriteLine($"i{index} - c{front.Count}");
                Console.WriteLine(nextGrid.ToString());
                
                // Backtrack if no successors.
                if (nextGrid is null)
                {
                    index--;
                    continue;
                }

                // Forward checking.
                bool successful = orm.ApplyForwardChecking((Grid) nextGrid, index);
                
                // Backtrack if empty domain.
                if (!successful)
                {
                    front.Push(grid);
                    continue;
                }
                
                // Wrap up.
                front.Push(nextGrid);
                index++;
            }
        }

        private Grid? GetSuccessor(Grid grid, int index)
        {
            Cell cell = grid.GetCell(index);
            
            // Skip predetermined and set cells.
            if (cell.set)
            {
                return this.GetSuccessor(grid, ++index);
            }
            
            // If all successors have already been checked.
            if (cell.DomainIsEmpty())
            {
                return null;
            }
            
            // Reduce successor pool of parent.
            int domainValue = cell.PopDomain();
            
            // Assign value in successor grid.
            Grid nextGrid = (Grid) grid.Clone();
            Cell nextCell = nextGrid.GetCell(index);
            nextCell.WriteValue(domainValue);
            return nextGrid;
        }
    }
}