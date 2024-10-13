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
        
        public readonly int Size;
        public int EmptyCell;
        public int Iterations;
        public bool Solved;
        public State Start;
        public State Finish;
        public List<State> Queue;
        public List<State> Checked;
        const int LimitLDFS=1000;
        const int LDFSMaxSteps=100000;
        public void SolveLDFS(){
            Iterations=0;
            if(!Start.IsSolvable()) throw new Exception("Deemed Unsolvable");
            InsertInQueue(Start);
            State current=Start;
            while(!current.IsSolved&&Queue.Count>0){
                Iterations++;
                Queue.RemoveAt(0);
                current.Unfold();
                foreach(State child in current.children){
                    if(child.Depth<LimitLDFS&&!IsStateInChecked(child)) Queue.Insert(0,child);
                }
                Checked.Add(current);
                if(Queue.Count==0) return;
                current=Queue[0];
                if (Iterations>LDFSMaxSteps) return;
            }
            if(current.IsSolved){
                Finish=current;
                Solved=true;
            }
        }
        public void SolveAstar(){
            Iterations=0;
            if(!Start.IsSolvable()) throw new Exception("Deemed Unsolvable");
            InsertInQueue(Start);
            State current=Start;
            while(!current.IsSolved){
                Iterations++;
                Queue.RemoveAt(0);
                current.Unfold();
                foreach(State child in current.children){
                    if(!IsStateInChecked(child)) InsertInQueue(child);
                }
                Checked.Add(current);
                current=Queue[0];
                }
            Finish=current;
            Solved=true;
            }   
        
        void InsertInQueue(State state){
            if(Queue.Count==0){
                Queue.Add(state);
                return;
            }
            if(state.Desirability<=Queue[0].Desirability){
                Queue.Insert(0,state);
                return;
            }
            if(state.Desirability>Queue[Queue.Count-1].Desirability){
                Queue.Insert(Queue.Count,state);
                return;
            }
            for(int i=0;i<Queue.Count-1;i++){
                if(Queue[i].Desirability<state.Desirability&&state.Desirability<=Queue[i+1].Desirability){
                    Queue.Insert(i+1,state);
                    return;
                }
            }
        }
        public bool IsStateInChecked(State state){
            foreach(State checking in Checked){
                if(state.Distance==checking.Distance){
                    if(Equal(checking, state)) {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool Equal(State a, State b){
            if(a is null &&b is null)return true;
            if(a is null || b is null) return false;
            if(a.Size!=b.Size) return false;
            for(int i=0;i<a.Size;i++){
                for(int j=0;j<a.Size;j++){
                    if(a[i,j]!=b[i,j]) return false;
                }
            }
            return true;
        }
    }   
}