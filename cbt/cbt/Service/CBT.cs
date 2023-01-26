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
        public void Run(State startState, ORM orm)
        {
            // Initialize Stack
            Stack front = new Stack();
            front.Push(startState);

            while (front.Count > 0 && this.focus < 81)
            {
                State current = (State) front.Pop();
                
                // Relax and get successor.
                current = this.PrepareRelaxation(current);
                State? maybeSuccessor = this.GetSuccessor(current);

                // Backtrack if no successors.
                if (maybeSuccessor is null)
                {
                    this.focus -= current.step;
                    continue;
                }
                
                // Forward checking.
                State successor = (State) maybeSuccessor;
                bool successful = orm.ApplyForwardChecking(successor.grid, this.focus);
                
                // Backtrack if empty domain.
                if (!successful)
                {
                    front.Push(current);
                    continue;
                }
                
                
                // Wrap up.
                front.Push(current);
                front.Push(successor);
                this.focus++;
            }

            State result = (State) front.Pop();
            Console.WriteLine(result);
        }

        private State PrepareRelaxation(State current)
        {
            Cell cell = current.grid.GetCell(this.focus);

            if (cell.set)
            {
                current.step++;
                this.focus++;
                return this.PrepareRelaxation(current);
            }
            
            current.GetNextSuccessorValue(cell);
            return current;
        }

        private State? GetSuccessor(State current)
        {
            if (current.noSuccessors)
            {
                return null;
            }

            State successor = (State) current.Clone();
            successor.grid.WriteCell(this.focus, current.successorValue);
            return successor;
        }
    }
}