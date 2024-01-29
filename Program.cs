using System;
using System.Text.RegularExpressions;

namespace Zombie19
{
    public class Program
    {
        Mechanics mechanics = new Mechanics();
        
        static void Main(string[] args)
        {
            Program program = new Program();
            program.StartSimulation();

            Console.WriteLine("\n\nPress any key to close the console...");
            Console.ReadKey();
        }        

        public void StartSimulation()
        {          
            Console.WriteLine("How many characters do you want to generate?");
            int.TryParse(Console.ReadLine(), out int numberCharacters);

            Console.WriteLine("How many groups do you want to generate?");
            int.TryParse(Console.ReadLine(), out int numberGroups);

            mechanics.CreateGroups(numberGroups);
            mechanics.CreateCharacters(numberCharacters, numberGroups);
            mechanics.CheckOneOrmOREInfected();
            mechanics.SortCharactersInGroups();
            mechanics.DisplayGroups();
            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey();
            mechanics.KeepWorking();
        }

        
    }
}