using System;
using EscapeGame;

class GameMaster
{
    public static void LoadOrNewGame()
    {
        do
        {
            Console.WriteLine("Please choose your option:");
            Console.WriteLine("\t1. New Game\n" +
                              "\t2. Load Last Game\n" +
                              "\t3. Instructions");

            string optionChosen = Console.ReadLine().Trim();           

            switch (optionChosen)
            {
                case "1":
                    Console.Clear();
                    GameBehaviour.CreateNewGame();                    
                    break;

                case "2":
                    Console.Clear();

                    try
                    {
                        GameBehaviour.LoadGame();
                    }
                    catch
                    {
                        Console.WriteLine("There is no save file to load!");
                        continue;
                    }
                    
                    Console.WriteLine("You find yourself in the {0} part of the Room.", GameObject.playerPosition);
                    GameBehaviour.ShowInventory();
                    break;

                case "3":
                    GameBehaviour.HelpMenu();
                    Console.Clear();
                    continue;

                default:
                    Console.WriteLine("Please choose exclusively 1, 2 or 3");
                    continue;
            }

            break;

        } while (true);
    }

    static void Main()
        {
        string playerInput;

        Console.WriteLine("Welcome Player!"); //Here goes general info about the game

        LoadOrNewGame();        

        do
        {
            if(GameObject.isCombinable[15] == GameObject.ItemCombined.isCombined)
            {
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine(GameBehaviour.ThanksAndGoodbye());
                Console.ReadLine();
                GameBehaviour.SaveAndExit();
                break;
            }
            else
            {
                Console.WriteLine("\nWhat do you wish to do?");
                playerInput = Console.ReadLine().Trim().ToLower();

                switch (playerInput)
                {
                    case "h":
                    case "help":
                        GameBehaviour.HelpMenu();
                        break;

                    case "c":
                    case "clear":
                        Console.Clear();
                        break;

                    case "x":
                    case "exit":
                        GameBehaviour.SaveAndExit();
                        break;

                    case "m":
                    case "move":
                        GameBehaviour.MoveChar();
                        break;

                    case "i":
                    case "inventory":
                        GameBehaviour.ShowInventory();
                        break;

                    case "p":
                    case "pick":
                        GameBehaviour.PickItem();
                        break;

                    case "e":
                    case "examine":
                        GameBehaviour.ExamineItem();
                        break;

                    case "u":
                    case "use":
                        GameBehaviour.UseItem();
                        break;

                    default:
                        Console.WriteLine("Command not recognised. Please try again.");
                        break;
                }
            }            

        } while ((playerInput != "exit") && (playerInput != "x"));

        Console.Clear();
        Console.WriteLine("You have succesfully saved and closed the game. Press Enter to exit to Windows.");
        Console.ReadLine();
    }
}