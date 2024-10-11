using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using _8_Puzzle_Console;

const int SIZE=3;

Game game;
do{
    game=new Game(SIZE);
    Console.WriteLine(game.Start.CountInversions());
}while(!game.Start.IsSolvable());

game.SolveLDFS();
List<int[,]> states=[game.Start.Tiles];
State current=game.Finish;
while(current!=game.Start){
    int[,]array=new int[SIZE,SIZE];
    for(int i=0;i<SIZE;i++){
        for(int j=0;j<SIZE;j++){
            array[i,j]=current[i,j];
        }
    }
    states.Insert(1,array);
    current=current.Parent;
}


foreach(int[,] array in states){
    string line="";
    for(int i=0;i<SIZE;i++){
        line+="[";
        for(int j=0;j<SIZE;j++){
            line+=array[i,j].ToString();
            if(j<SIZE-1) line+=", ";
        }
        line+="]\n";
    }
    line+="\n";
    Console.WriteLine(line);
}

Console.WriteLine($"Number of iterations: {game.Iterations}");
Console.WriteLine($"Steps: {states.Count-1}");