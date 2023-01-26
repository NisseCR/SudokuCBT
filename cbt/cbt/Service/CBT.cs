using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sudoku.Model;

namespace Sudoku.Service
{
    public class CBT
    {
        private int focus;
        
        /// <summary>
        /// Tool to measure execution time.
        /// </summary>
        private Stopwatch watch;
        
        /// <summary>
        /// Project directory.
        /// </summary>
        private static string dir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        /// <summary>
        /// Log file path.
        /// </summary>
        private static string filePath = @$"{dir}\Temp\log.txt";
        
        /// <summary>
        /// Log of focus values, used to visualise the progress of the algorithm.
        /// </summary>
        public List<int> log;

        private ORM orm;

        public CBT(ORM orm)
        {
            this.focus = 0;
            this.watch = new Stopwatch();
            this.log = new();
            this.orm = orm;
        }

        /// <summary>
        /// Perform DFS with Backtracking & Forward checking on a incomplete Sudoku puzzle.
        /// </summary>
        /// <param name="grid">The incomplete Sudoku puzzle</param>
        /// <param name="startState"></param>
        /// <param name="orm"></param>
        public void Run(State startState)
        {
            this.watch.Start();
            
            // Initialize Stack
            Stack<State> front = new Stack<State>();
            front.Push(startState);

            while (this.focus < 81)
            {
                // Log
                log.Add(this.focus);
                
                // Get current node.
                State current = front.Pop();
                Cell cell = this.GetFocusCell(current);
                
                // Skip set cells.
                if (this.SkipSet(front, current, cell))
                {
                    continue;
                }

                // Relax and get successor.
                (State, State?) relaxResult = this.Relax(current);
                current = relaxResult.Item1;
                State? maybeSuccessor = relaxResult.Item2;

                // Backtrack if no successors.
                if (this.BacktrackNoSuccessor(current, maybeSuccessor))
                {
                    continue;
                }
                
                // Backtrack if node consistency results in empty domain.
                State successor = (State) maybeSuccessor;
                if (this.BacktrackEmptyDomain(front, current, successor))
                {
                    continue;
                }

                // Add path and successor to front.
                front.Push(current);
                front.Push(successor);
                this.focus++;
            }
            
            this.watch.Stop();
            State solution = (State) front.Pop();
            this.WriteResult(solution);
        }

        private Cell GetFocusCell(State current)
        {
            return current.grid.GetCell(this.focus);
        }

        private bool SkipSet(Stack<State> front, State current, Cell cell)
        {
            if (!cell.set)
            {
                return false;
            }
            
            // Focus on next cell for instantiating value.
            current.step++;
            this.focus++;
            front.Push(current);
            return true;
        }

        private bool BacktrackNoSuccessor(State current, State? maybeSuccessor)
        {
            if (!(maybeSuccessor is null))
            {
                return false;
            }
            
            // Backtrack focus.
            this.focus -= current.step;
            return true;
        }

        private bool BacktrackEmptyDomain(Stack<State> front, State current, State successor)
        {
            if (this.orm.ApplyForwardChecking(successor.grid, this.focus))
            {
                return false;
            }
            
            front.Push(current);
            return true;
        }

        private State PrepareRelaxation(State current)
        {
            Cell cell = current.grid.GetCell(this.focus);
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

        private (State, State?) Relax(State current)
        {
            current = this.PrepareRelaxation(current);
            State? maybeSuccessor = this.GetSuccessor(current);
            return (current, maybeSuccessor);
        }
        
        /// <summary>
        /// Export the log date of h-values to file.
        /// </summary>
        public void Export()
        {
            List<string> data = this.log.Select(i => i.ToString()).ToList();
            File.WriteAllLines(CBT.filePath, data);
        }
        
        /// <summary>
        /// Print the solution.
        /// </summary>
        private void WriteResult(State state)
        {
            Console.WriteLine($"Solution:\nExecution Time: {watch.ElapsedMilliseconds} ms {state.grid}");
        }
    }
}