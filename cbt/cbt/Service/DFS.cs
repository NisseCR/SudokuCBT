using System;
using System.Collections;
using System.Collections.Generic;
using Sudoku.Model;

namespace Sudoku.Service
{
    public class DFS
    {
        // DFS w/ backtracking front and neighbour selection
        // 2 cases where backtracking is required:
        //  - Not a partial solution (mayhaps not need)
        //  - Empty domain in forward checking
        // Mikey

        //TO DO
        // Create stack and start with beginstate
        // Expand every available option in domain in first variable 
        //
        // For successors
        //   if:
        //    - Not a partial solution (mayhaps not need)
        //    - Empty domain in forward checking
        //   Don't expand successor
        //  
        // Continue process until state with last index is reached

        /// <summary>
        /// Perform DFS with Backtracking & Forward checking on a incomplete Sudoku puzzle
        /// </summary>
        /// <param name="state">The incomplete Sudoku puzzle</param>
        public void Run(State state)
        {

            // Initialize Stack
            Stack CBT = new Stack();
            CBT.Push(state);

            State currentState = null;

            while (CBT.Count > 0)
            {
                currentState = (State)CBT.Pop();

                // Goal State
                if (false) // If the last index of the array is reached
                {
                    break;
                }

                List<State> successors = GetSuccessors(currentState);

                foreach(State successor in successors)
                {
                    CBT.Push(successor);
                }

            }
            
            Console.WriteLine(currentState);
        }

        // Use domain of current index of state and for every available option
        // Check if
        //  - Not a partial solution (mayhaps not need)
        //  - Empty domain in forward checking

        /// <summary>
        /// Create a list of valid successors from a state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<State> GetSuccessors(State state)
        {
            State currentState = state;
            List<State> successors = new List<State>();

            // Get all available domain options and loop until none left
            while (false)
            {
                /*
                //Create successor state with new domain option
                State newSuccessor = new state;

                if (BacktrackCheck(newSuccessor) && ForwardCheck(newSuccessor))
                {
                    successors.Add(newSuccessor);
                }
                */
            }

            return successors;
        }

        public bool BacktrackCheck(State state)
        {
            if (false) // Check if state is a partial solution
            {
                return true;
            }
            return false;
        }

        public bool forwardCheck(State state)
        {
            if (false) // Check if state has no empty domains
            {
                return true;
            }
            return false;
        }
    }
}