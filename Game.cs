using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle_Console {
    public class Geme{
        public Game():this(3) { }
        public Game(int size):this(new State(size))  { }
        public Game(int[,] matrix) { }
        public Game(State start) { }
    }   
}