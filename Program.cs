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


void OutputIntoConsole(){
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
}
State StateFromConsole(){
    bool Okay=false;
    int size=0;
    do{
        try{
            Console.WriteLine("Length in squares:");
            size=int.Parse(Console.ReadLine());
            Okay=true;
            if(size<=0||size>4){
                Console.WriteLine("Write between 1 and 4");
            }
        }
        catch{
            Console.WriteLine("Write only a number");    
        }
    }while(!Okay);
    int [,] Array=new int[size,size];
    for(int i=0;i<size*size;i++){
        int current=-1;
        do{
            Okay=true;
            Console.WriteLine(ArrayToStringTable(Array,size,i));
            Console.WriteLine("Input number: ");
            try{
                current = int.Parse(Console.ReadLine());
            }
            catch{
                Console.WriteLine("Number was not detected. Try again");
                Okay=false;
            }
            for(int m=0;m<i;m++){
                if(Array[m*size,m%size]==current){
                    Console.WriteLine("Number isn't unique");
                    Okay=false;
                }
            }
            if(current<0||current>=size*size){
                Console.WriteLine($"Number must be from 0 to {size*size-1}");
            }
        } while (!Okay);
        Array[i/size,i%size]=current;
    }
    return new State(size, Array);
}
string ArrayToStringTable(int[,] arr, int size, int place){
    string line="";
    for(int i=0;i<size;i++){
        line+="[";
        for(int j=0;j<size;j++){
            if(i*size+j<place) line+=arr[i,j].ToString();
            if(i*size+j==place) line+="?";
            if(i*size+j>place) line+="_";
            if(j<SIZE-1) line+=", ";
        }
        line+="]\n";
    }
    return line;
}