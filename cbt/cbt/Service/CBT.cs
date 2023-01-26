using System;
using System.Collections;
using System.Collections.Generic;
using Sudoku.Model;

namespace Sudoku.Service
{
    public class CBT
    {

        private int focus;

        public CBT()
        {
            this.focus = 0;
        }

        /// <summary>
        /// Perform DFS with Backtracking & Forward checking on a incomplete Sudoku puzzle
        /// </summary>
        /// <param name="grid">The incomplete Sudoku puzzle</param>
        public void Run(Grid grid, ORM orm)
        {

            // Initialize Stack
            Stack front = new Stack();
            front.Push(grid);
            this.focus = 0;

            Grid current;
            while (front.Count > 0 && this.focus < 81)
            {
                // Get DFS successor.
                current = (Grid) front.Pop();
                Grid? successor = this.GetSuccessor(current);

                // Debug
                Console.WriteLine($"i{this.focus} - c{front.Count}");
                Console.WriteLine(current.ToString());
                
                // Backtrack if no successors.
                if (successor is null)
                {
                    this.focus--;
                    Console.WriteLine("No successors --> bt");
                    continue;
                }
                
                // Forward checking.
                bool successful = orm.ApplyForwardChecking((Grid) successor, this.focus);
                
                // Backtrack if empty domain.
                if (!successful)
                {
                    front.Push(current);
                    Console.WriteLine("Successorsfutfffff:");
                    Console.WriteLine(successor.ToString());
                    Console.WriteLine("Failed fc --> bt");
                    continue;
                }
                
                
                // Wrap up.
                front.Push(successor);
                this.focus++;
            }
        }

        private int? GetSuccessorValue(Grid grid)
        {
            Cell cell = grid.GetCell(this.focus);
            
            // Skip predetermined and set cells.
            if (cell.set)
            {
                this.focus++;
                return this.GetSuccessorValue(grid);
            }
            
            // Reduce successor pool of parent.
            return cell.PopDomain();
        }

        private Grid ApplySuccessorValue(Grid source, int successorValue)
        {
            Grid grid = (Grid) source.Clone();
            grid.WriteCell(this.focus, successorValue);
            return grid;
        }

        private Grid? GetSuccessor(Grid grid)
        {
            int? successorValue = this.GetSuccessorValue(grid);

            if (successorValue is null)
            {
                return null;
            }

            return this.ApplySuccessorValue(grid, (int) successorValue);
        }
    }
}