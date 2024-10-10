using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle_Console {
    public class Game{
        public Game():this(3) { }
        public Game(int size):this(new State(size))  { }
        public Game(int[,] matrix): this(new State(matrix.Length,matrix)) { }
        public Game(State start) { 
            Start=start;
            Size=start.Size;
            EmptyCell=start.EmptyCell;
            Queue=new List<State>();
            Checked=new List<State>();
        }
        State Start;
        public readonly State Finish;
        List<State> Queue;
        List<State> Checked;
        public int EmptyCell;
        int Size;
    }   
    
}