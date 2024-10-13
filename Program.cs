using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using _8_Puzzle_Console;

State start;
if(FirstOrSecond("How do you want to get table set up?","Auto generate", "Type yourself")==1){
    int size=GetSize();
    do{
    start=new State(size);
    }while (!start.IsSolvable());
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
Stopwatch sw=new Stopwatch();
sw.Start();
if(FirstOrSecond("Which method to use?", "LDFS", "A*")==1){
    Console.WriteLine(ArrayToStringTable(game.Start.Tiles, game.Size, game.Size*game.Size));
    game.SolveLDFS();
}
else{
    Console.WriteLine(ArrayToStringTable(game.Start.Tiles, game.Size, game.Size*game.Size));
    sw=new Stopwatch();
    game.SolveAstar();
}
sw.Stop();
if(game.Finish is not null&&game.Finish.IsSolved){
    OutputIntoConsole(game);
    Console.WriteLine($"Time - {sw.ElapsedMilliseconds} miliseconds");
    Console.WriteLine(ArrayToStringTable(game.Start.Tiles, game.Size, game.Size*game.Size));
}
else{
    Console.WriteLine("Solution wasn't found");
    Console.WriteLine($"Time - {sw.ElapsedMilliseconds} miliseconds");
}

void OutputIntoConsole(Game game){
    int size=game.Size;
    List<int[,]> states=[game.Start.Tiles];
    State current=game.Finish;
    while(current!=game.Start){
        int[,]array=new int[size,size];
        for(int i=0;i<size;i++){
            for(int j=0;j<size;j++){
                array[i,j]=current[i,j];
            }
        }
        states.Insert(1,array);
        current=current.Parent;
    }


    foreach(int[,] array in states){
        string line="";
        for(int i=0;i<size;i++){
            line+="[";
            for(int j=0;j<size;j++){
                line+=array[i,j].ToString();
                if(j<size-1) line+=", ";
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
                if(Array[m/size,m%size]==current){
                    Console.WriteLine("Number isn't unique");
                    Okay=false;
                }
            }
            if(current<0||current>=size*size){
                Console.WriteLine($"Number must be from 0 to {size*size-1}");
                Okay=false;
            }
        } while (!Okay);
        Array[i/size,i%size]=current;
    }
    Console.WriteLine(ArrayToStringTable(Array, size, size*size));
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
            if(j<size-1) line+=", ";
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


/*State start=new State(3,new int[,]{{1,4,2},{0,7,3},{5,8,6}});
State reference=new State(3, new int[,]{{1,2,3},{0,4,6},{7,5,8}});
State.reference=reference;
Game game=new Game(start);
try{
    game.SolveAstar();
    Console.WriteLine("You did it!!!!");
}
catch{
    Console.WriteLine("Wasn't solved properly");
}
Console.WriteLine("Search begins");
foreach(State checking in game.Checked){
    if(game.IsStateInChecked(reference)){
        Console.WriteLine($"Bastard was found! {checking.Desirability-checking.Depth}");
    };
    for(int i=0;i<8;i++){
        for (int j=i+1;j<9;j++){
            if (checking[i/3,i%3]==checking[j/3,j%3]) {
                Console.WriteLine("A number repeated was found!");
            };
        }
    }
}
game.InsertInQueue(reference);
if(!game.IsStateInChecked(reference)){
    Console.WriteLine($"It can't inert properly");
};
Console.WriteLine("We didn't find him");

bool Equal(State a, State b){
    int Count=0;
    for(int i=0;i<3;i++){
        for(int j=0;j<3;j++){
            if(a[i,j]==b[i,j]) Count++;
        }
    }
    if(Count==9){
        Console.WriteLine("stop");
    }
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
*/
