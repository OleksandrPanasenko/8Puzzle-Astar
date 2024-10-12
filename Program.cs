using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using _8_Puzzle_Console;

const int SIZE=3;
State start;
if(FirstOrSecond("How do you want to get table set up?","Auto generate", "Type yourself")==1){
    start=new State(GetSize());
}
else{
    do{
        start=StateFromConsole();
        if(!start.IsSolvable()){
            Console.WriteLine("This is unsolvable. Try something again");
        }
    }while(!start.IsSolvable());
}
Game game=new Game(start);
Stopwatch sw;
if(FirstOrSecond("Which method to use?", "LDFS", "A*")==1){
    sw=new Stopwatch();
    game.SolveLDFS();
}
else{
    sw=new Stopwatch();
    game.SolveAstar();
}

OutputIntoConsole(game);
Console.WriteLine($"Time - {sw.ElapsedMilliseconds/1000} seconds");


void OutputIntoConsole(Game game){
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
int GetSize(){
    int size=0;
    bool Okay=false;
    do{
        Okay=true;
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
    return size;
}
State StateFromConsole(){
    int size=GetSize();
    bool Okay=false;
    
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
int FirstOrSecond(string message, string first, string second){
    string input;
    do{
        Console.WriteLine(message);
        Console.WriteLine($"1 - {first}\n 2 - {second}");
        input=Console.ReadLine();
        if(input!="1"&&input!="2") Console.WriteLine("Write '1' or '2'");
    }while(input!="1"&&input!="2");
    return int.Parse(input);
}

