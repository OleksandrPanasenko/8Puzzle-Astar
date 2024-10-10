using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle_Console
{
    public class State
    {
        public State():this(3){}
        public State(int size){
            int[,] Matrix=new int[size,size];
            List<int> list=new List<int>();
            for(int i=0; i<size*size;i++) list.Add(i);
            Random random=new Random();
            for (int i=0; i<size*size; i++){
                int j=random.Next(0,size);
                if(j!=i){
                    list[i]=list[i]+list[j];
                    list[j]=list[i]-list[j];
                    list[i]=list[i]-list[j];
                }
            };
            for(int i=0; i<size; i++){
                for(int j=0; j<size;j++){
                    Matrix[i,j] = list[size*i+j];
                }
            }
            Tiles=Matrix;
            Size=size;
            Depth = 0;
            Distance = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Tiles[i,j]!=EmptyCell&&Tiles[i,j]!=Size* i+j + EmptyCell) Distance++;
                }
            }
            StartTile=1;
        }
        public State(int size, int[,] tiles)
        {
            Size = size;
            Tiles = new int[size,size];
            Depth = 0;
            Distance = 0;
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Tiles[i,j]=tiles[i,j];
                    if (tiles[i,j]!=EmptyCell&&tiles[i,j]!=Size* i+j + EmptyCell) Distance++;
                }
            }
            children=null;
            StartTile=1;
        }
        public State(int size, int[,] tiles, int EmptyNumber):this(size, tiles){
            StartTile=EmptyNumber+1;
        }
        public State (int size, int[,] tiles, State parent):this(size, tiles){
            Parent=parent;
            Depth=parent.Depth+1;
            StartTile=parent.StartTile;
        }
        public void Unfold(){
            children=new List<State>();
            int row=-1;
            int col=-1;
            if(!Unfolded){
                for(int i=0; i<Size; i++){
                    for(int j=0; j<Size;j++){
                        if(Tiles[i,j]==StartTile-1){
                            row=i;
                            col=j;
                            break;
                        }
                    }
                }
                if(row==-1) throw new Exception("Unfold didn't find the empty cell");
                //тайл вгору
                if(row<Size-1){
                    Tiles[row,col]=Tiles[row+1,col];
                    Tiles[row+1,col]=StartTile-1;
                    children.Add(new State(Size, Tiles, this));
                    Tiles[row+1,col]=Tiles[row,col];
                    Tiles[row,col]=StartTile-1;
                }
                //тайл вниз
                if(row>0){
                    Tiles[row,col]=Tiles[row-1,col];
                    Tiles[row-1,col]=StartTile-1;
                    children.Add(new State(Size, Tiles, this));
                    Tiles[row-1,col]=Tiles[row,col];
                    Tiles[row,col]=StartTile-1;
                }
                //тайл ліворуч
                if(col<Size-1){
                    Tiles[row,col]=Tiles[row,col+1];
                    Tiles[row,col+1]=StartTile-1;
                    children.Add(new State(Size, Tiles, this));
                    Tiles[row,col+1]=Tiles[row,col];
                    Tiles[row,col]=StartTile-1;
                }
                //тайл праворуч
                if(col>0){
                    Tiles[row,col]=Tiles[row,col-1];
                    Tiles[row,col-1]=StartTile-1;
                    children.Add(new State(Size, Tiles, this));
                    Tiles[row,col-1]=Tiles[row,col];
                    Tiles[row,col]=StartTile-1;
                }
                Unfolded=true;
            }
        }
    
        public readonly int Size;
        int[,] Tiles;
        public List<State> children;
        int Depth;
        int Distance;
        public bool IsSolved{get{return Distance==0;}}
        public int Desirability{
                get{
                    return Depth + Distance;
                }
            }
        int StartTile = 1;
        public int EmptyCell{get{return StartTile-1;}}
        bool Unfolded=false;
        public State Parent;
        public int this[int i, int j]
        {
            get {return Tiles[i,j]; }
            set {Tiles[i,j]=value;  }
        }
    }
}
