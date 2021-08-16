using System;

namespace EscapeGame
{
    public class GameBehaviour
    {
        /// <summary>
        /// Creates the standard state of all game objects at the beginning of the game.
        /// </summary>
        public static void CreateNewGame()
        {
            GameObject.playerPosition = GameObject.RoomLocation.Entrance;

            //intro text
            Console.WriteLine("You wake up... seems like you have overslept through most of the morning and into noon,\n" +
                              "judging from the sun outside your window. Flashes of last night's party come to your mind\n" +
                              "as you suddenly realize how dry your mouth is and how much your head hurts from hangover.\n" +
                              "You get up, thinking of heading to the kitchen to grab a bite to calm your angry stomach.\n\n" +
                              "However, as you try to pull your room's door open you realize that is locked!\n" +
                              "As your numb brain tries to make sense of the situation a sudden memory of last night rushes\n" +
                              "to your mind... You played a rather borderline prank on your friend Marvin and as retribution\n" +
                              "he must have for sure locked the door. You slam it and yell for a while, but there is no answer from outside...\n\n" +
                              "You sigh to yourself, leaning your head agains the door. If there are spare copies of the keys,\n" +
                              "they must be for sure in the basement... and mentalize yourself of having to stay there until\n" +
                              "Marvin decides that you have suffered enough.\n\n" +
                              "After what it feels like an eternity leaning at the door a distant childhood memory strikes you\n" +
                              "seemingly from nowhere: you loved to create riddles and puzzles with your father and hide\n" +
                              "stuff throughout the house to play treasure hunt with him. Sometimes also spare keys.\n" +
                              "Maybe you hid a spare key somewhere in your room? You are sure of it, because he jokingly complained\n" +
                              "about it for years before passing away. However, it was so long ago that you barely remember the details.\n\n" +
                              "You turn around and look at your L-shaped Room: the Entrance part,the Center part and the Back part\n" +
                              "and decide to look for clues... After all you are going to be here for a while anyway,\n" +
                              "so it's better if you keep yourself occupied with something instead of idly looking at the ceiling.\n" +
                              "Maybe you might find something in the wardrobe or the desk? You wonder to yourself...\n");
            Console.WriteLine("You find yourself in the {0} part of the Room.", GameObject.playerPosition);
        }

        /// <summary>
        /// Saves the current state of the game
        /// </summary>
        public static void SaveAndExit()
        {
            System.IO.StreamWriter gameStateSaver = new System.IO.StreamWriter("Save_File_Escape_The_Room.txt");

            try
            {
                gameStateSaver.WriteLine(GameObject.playerPosition.ToString());

                gameStateSaver.WriteLine(GameObject.playerInventory.Count);
                foreach(string item in GameObject.playerInventory)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.isOpenable.Length);
                foreach (GameObject.ItemClosedOrOpen item in GameObject.isOpenable)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.isSwitchable.Length);
                foreach (GameObject.ItemOnOrOff item in GameObject.isSwitchable)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.isCombinable.Length);
                foreach (GameObject.ItemCombined item in GameObject.isCombinable)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.objectsInCenter.Length);
                foreach (string item in GameObject.objectsInCenter)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.objectsInBack.Length);
                foreach (string item in GameObject.objectsInBack)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.objectsInEntrance.Length);
                foreach (string item in GameObject.objectsInEntrance)
                {
                    gameStateSaver.WriteLine(item);
                }

                gameStateSaver.WriteLine(GameObject.objectsInRoof.Length);
                foreach (string item in GameObject.objectsInRoof)
                {
                    gameStateSaver.WriteLine(item);
                }
            }
            catch
            {
                Console.WriteLine("Save failed!");
            }
            finally
            {
                gameStateSaver.Close();
            }
        }

        /// <summary>
        /// Loads the game to the previous state.
        /// </summary>
        public static void LoadGame()
        {
            System.IO.StreamReader gameStateLoader = new System.IO.StreamReader("Save_File_Escape_The_Room.txt");

            try
            {
                string playerPositionString = gameStateLoader.ReadLine();
                GameObject.playerPosition = Enum.Parse<GameObject.RoomLocation>(playerPositionString);

                string itemsInInventoryString = gameStateLoader.ReadLine();
                int itemsInInventory = int.Parse(itemsInInventoryString);
                for(int i = 0; i < itemsInInventory; i++)
                {
                    GameObject.playerInventory.Add(gameStateLoader.ReadLine());
                }

                string itemsInIsOpenableString = gameStateLoader.ReadLine();
                int itemsInIsOpenable = int.Parse(itemsInIsOpenableString);
                for (int i = 0; i < itemsInIsOpenable; i++)
                {
                    GameObject.isOpenable[i] = Enum.Parse
                        <GameObject.ItemClosedOrOpen>(gameStateLoader.ReadLine());
                }

                string itemsInIsSwitchableString = gameStateLoader.ReadLine();
                int itemsInIsSwitchable = int.Parse(itemsInIsSwitchableString);
                for (int i = 0; i < itemsInIsSwitchable; i++)
                {
                    GameObject.isSwitchable[i] = Enum.Parse
                        <GameObject.ItemOnOrOff>(gameStateLoader.ReadLine());
                }

                string itemsInIsCombinableString = gameStateLoader.ReadLine();
                int itemsInIsCombinable = int.Parse(itemsInIsCombinableString);
                for (int i = 0; i < itemsInIsCombinable; i++)
                {
                    GameObject.isCombinable[i] = Enum.Parse
                        <GameObject.ItemCombined>(gameStateLoader.ReadLine());
                }

                string itemsInObjectsInCenterString = gameStateLoader.ReadLine();
                int itemsInObjectsInCenter = int.Parse(itemsInObjectsInCenterString);
                for (int i = 0; i < itemsInObjectsInCenter; i++)
                {
                    GameObject.objectsInCenter[i] = gameStateLoader.ReadLine();
                }

                string itemsInObjectsInBackString = gameStateLoader.ReadLine();
                int itemsInObjectsInBack = int.Parse(itemsInObjectsInBackString);
                for (int i = 0; i < itemsInObjectsInBack; i++)
                {
                    GameObject.objectsInBack[i] = gameStateLoader.ReadLine();
                }

                string itemsInObjectsInEntranceString = gameStateLoader.ReadLine();
                int itemsInObjectsInEntrance = int.Parse(itemsInObjectsInEntranceString);
                for (int i = 0; i < itemsInObjectsInEntrance; i++)
                {
                    GameObject.objectsInEntrance[i] = gameStateLoader.ReadLine();
                }

                string itemsInObjectsInRoofString = gameStateLoader.ReadLine();
                int itemsInObjectsInRoof = int.Parse(itemsInObjectsInRoofString);
                for (int i = 0; i < itemsInObjectsInRoof; i++)
                {
                    GameObject.objectsInRoof[i] = gameStateLoader.ReadLine();
                }
            }
            catch
            {
                Console.WriteLine("Loading failed!");
            }
            finally
            {
                gameStateLoader.Close();
            }
        }

        /// <summary>
        /// Shows the game's instructions to the player. Accessed by typing "h" or "help".
        /// </summary>
        public static void HelpMenu()
        {
            Console.WriteLine("Escape the Room is a text-based adventure game where the objective is to get out of your room.\n" +
                              "For that you will need to examine and interact with your environment.\n" +
                              "This is done through different commands you need to type in.\n" +
                              "The different commands are listed in the next page.\n" +
                              "You will also have to type objects or locations. For simplicity you will never have to type more\n" +
                              "than one word at a time. No compound commands are used in this game.\n" +
                              "As a hint, some objects that you can interact with may start with an uppercase in descriptions.\n" +
                              "However, as a rule of thumb, if there is a noun in a description of something,\n" +
                              "chances are that you can examine/interact with it in some way.\n\n" +
                              "The key here is to experiment and have fun!\n");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            Console.WriteLine("These are all possible commands you can type. Shortcuts to them are marked in uppercase.");
            Console.WriteLine("Type \"Help\" to call this menu back at any given time during the game.");
            Console.WriteLine("Type \"Clear\" to clear the screen.");
            Console.WriteLine("Type \"eXit\" to save and exit the game");
            Console.WriteLine("Type \"Move\" to move to the different parts of the room.");
            Console.WriteLine("Type \"Examine\" to have a closer look at a given item.");
            Console.WriteLine("Type \"Use\" to use an item.");
            Console.WriteLine("Type \"Pick\" to pick up an item.");
            Console.WriteLine("Type \"Inventory\" to see your inventory.\n");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// This moves the player around the different parts of the room.
        /// </summary>
        public static void MoveChar()
        {
            do
            {
                Console.WriteLine("\nWhere do you want to move?");
                string charDestinationString = Console.ReadLine().Trim().ToLower();
                GameObject.RoomLocation charDestination;

                try
                {
                    charDestination = Enum.Parse<GameObject.RoomLocation>(charDestinationString, true);
                }
                catch
                {
                    Console.WriteLine("There is not such a place you can move to in your Room.");
                    continue;
                }
                

                if (charDestination == GameObject.playerPosition)
                {
                    Console.WriteLine("You find yourself already there.");
                    continue;
                }
                else
                {
                    switch (charDestination)
                    {
                        case GameObject.RoomLocation.Center:                            
                            if (GameObject.playerPosition == GameObject.RoomLocation.Roof)
                            {                                
                                Console.WriteLine("You carefully go down the ladder, grab the rope and swiftly jump into your room.\n" +
                                    "You then move across your Room towards the center.");
                                GameObject.playerPosition = GameObject.RoomLocation.Center;
                            }
                            else
                            {
                                Console.WriteLine("You move across your Room towards the center.");
                                GameObject.playerPosition = GameObject.RoomLocation.Center;
                            }                            
                            break;

                        case GameObject.RoomLocation.Back:                            
                            if (GameObject.playerPosition == GameObject.RoomLocation.Roof)
                            {
                                Console.WriteLine("You carefully go down the ladder, grab the rope and swiftly jump into your room.\n" +
                                    "You then move across your Room towards the Back.");
                                GameObject.playerPosition = GameObject.RoomLocation.Back;
                            }
                            else
                            {
                                Console.WriteLine("You move across your Room towards the Back.");
                                GameObject.playerPosition = GameObject.RoomLocation.Back;
                            }
                            break;

                        case GameObject.RoomLocation.Entrance:                            
                            if (GameObject.playerPosition == GameObject.RoomLocation.Roof)
                            {
                                Console.WriteLine("You carefully go down the ladder, grab the rope and swiftly jump into your room.\n" +
                                    "You then move across your Room towards the Entrance.");
                                GameObject.playerPosition = GameObject.RoomLocation.Entrance;
                            }
                            else
                            {
                                Console.WriteLine("You move across your Room towards the Entrance.");
                                GameObject.playerPosition = GameObject.RoomLocation.Entrance;
                            }
                            break;

                        case GameObject.RoomLocation.Roof:
                            if((GameObject.isCombinable[12] == GameObject.ItemCombined.isCombined) &&
                                (GameObject.playerInventory.Contains("gear")))
                            {
                                GameObject.playerPosition = GameObject.RoomLocation.Roof;
                                Console.WriteLine("You move across your room and lean out the window next to your bed.\n" +
                                    "You put on your gear and hiking shoes and attach yourself to the rope.\n" +
                                    "You grab the rope very carefully. With a quick, decisive move you get a hold of the ladder\n" +
                                    "and before you realize it you find yourself in the roof.");
                            }
                            else
                            {
                                Console.WriteLine("You can't go there, it's too risky!");
                            }
                            break;
                    }                    
                }

                break;

            } while (true);           
        }

        /// <summary>
        /// It looks if an item is in the part of the room where the player is and returns its description.
        /// </summary>
        /// <param name="location">The part of the room the player is in.</param>
        /// <param name="item">The item to examine.</param>
        public static string FindItemToExamine(GameObject.RoomLocation location, string item)
        {
            if (GameObject.playerInventory.Contains(item))
            {
                return GameObject.ItemDescriptionInventory(item);
            }

            if (location == GameObject.RoomLocation.Center)
            {
                for (int i = 0; i < GameObject.objectsInCenter.Length; i++)
                {
                    if (GameObject.objectsInCenter[i] == item)
                    {
                        return GameObject.ItemDescriptionCenter(item);
                    }
                }                
            }

            if (location == GameObject.RoomLocation.Back)
            {
                for (int i = 0; i < GameObject.objectsInBack.Length; i++)
                {
                    if (GameObject.objectsInBack[i] == item)
                    {
                        return GameObject.ItemDescriptionBack(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Entrance)
            {
                for (int i = 0; i < GameObject.objectsInEntrance.Length; i++)
                {
                    if (GameObject.objectsInEntrance[i] == item)
                    {
                        return GameObject.ItemDescriptionEntrance(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Roof)
            {
                for (int i = 0; i < GameObject.objectsInRoof.Length; i++)
                {
                    if (GameObject.objectsInRoof[i] == item)
                    {
                        return GameObject.ItemDescriptionRoof(item);
                    }
                }
            }

            return "There is no such item here.";
        }

        /// <summary>
        /// It allows the player to examine the different objects and room parts.
        /// </summary>
        public static void ExamineItem()
        {
            Console.WriteLine("\nWhat do you want to examine?");
                string itemToExamine = Console.ReadLine().Trim().ToLower();

            Console.WriteLine(FindItemToExamine(GameObject.playerPosition, itemToExamine));
        }

        /// <summary>
        /// It looks if an item is in the part of the room where the player is and returns the specific
        /// behaviour for that item as a string.
        /// </summary>
        /// <param name="location">The part of the room where the player is.</param>
        /// <param name="item">The specific item to use.</param>
        /// <returns></returns>
        public static string FindItemToUse(GameObject.RoomLocation location, string item)
        {
            if (GameObject.playerInventory.Contains(item))
            {
                return GameObject.UseItemInventory(item);
            }

            if (location == GameObject.RoomLocation.Center)
            {
                for (int i = 0; i < GameObject.objectsInCenter.Length; i++)
                {
                    if (GameObject.objectsInCenter[i] == item)
                    {
                        return GameObject.UseItemCenter(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Back)
            {
                for (int i = 0; i < GameObject.objectsInBack.Length; i++)
                {
                    if (GameObject.objectsInBack[i] == item)
                    {
                        return GameObject.UseItemBack(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Entrance)
            {
                for (int i = 0; i < GameObject.objectsInEntrance.Length; i++)
                {
                    if (GameObject.objectsInEntrance[i] == item)
                    {
                        return GameObject.UseItemEntrance(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Roof)
            {
                for (int i = 0; i < GameObject.objectsInRoof.Length; i++)
                {
                    if (GameObject.objectsInRoof[i] == item)
                    {
                        return GameObject.UseItemRoof(item);
                    }
                }
            }

            return "There is no such item here."; ;
        }

        /// <summary>
        /// Allows the player to interact with the sorroundings, using items for certain purposes.
        /// </summary>
        public static void UseItem()
        {
            Console.WriteLine("\nWhat do you want to use?");
                string itemToUse = Console.ReadLine().Trim().ToLower();

            Console.WriteLine(FindItemToUse(GameObject.playerPosition, itemToUse));
        }

        /// <summary>
        /// It looks if an item is in the part of the room where the player is and returns if
        /// the player has picked it or not.
        /// </summary>
        /// <param name="location">The part of the room where the player is.</param>
        /// <param name="item">The specific item to pick.</param>
        /// <returns></returns>
        public static string FindItemToPick(GameObject.RoomLocation location, string item)
        {
            foreach(string s in GameObject.playerInventory)
            {
                if(s == item)
                {
                    return "You already picked that item.";
                }
            }
            
            if (location == GameObject.RoomLocation.Center)
            {
                for (int i = 0; i < GameObject.objectsInCenter.Length; i++)
                {
                    if (GameObject.objectsInCenter[i] == item)
                    {
                        return GameObject.PickItemCenter(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Back)
            {
                for (int i = 0; i < GameObject.objectsInBack.Length; i++)
                {
                    if (GameObject.objectsInBack[i] == item)
                    {
                        return GameObject.PickItemBack(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Entrance)
            {
                for (int i = 0; i < GameObject.objectsInEntrance.Length; i++)
                {
                    if (GameObject.objectsInEntrance[i] == item)
                    {
                        return GameObject.PickItemEntrance(item);
                    }
                }
            }

            if (location == GameObject.RoomLocation.Roof)
            {
                for (int i = 0; i < GameObject.objectsInRoof.Length; i++)
                {
                    if (GameObject.objectsInRoof[i] == item)
                    {
                        return GameObject.PickItemRoof(item);
                    }
                }
            }

            return "There is no such item here."; ;
        }

        /// <summary>
        /// Allows the player to pick up items from the surroundings
        /// </summary>
        public static void PickItem()
        {
            Console.WriteLine("\nWhat do you want to pick up?");
                string itemToPick = Console.ReadLine().Trim().ToLower();

            Console.WriteLine(FindItemToPick(GameObject.playerPosition, itemToPick));
        }

        /// <summary>
        /// Shows the current items in the player's inventory.
        /// </summary>
        public static void ShowInventory()
        {
            if(GameObject.playerInventory.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
            }
            else
            {
                Console.WriteLine("\nYour inventory contains:");
                foreach (string item in GameObject.playerInventory)
                {
                    Console.WriteLine(item);
                }
            }            
        }
        /// <summary>
        /// Ending credits or the game
        /// </summary>
        /// <returns>The ending credits after the player beats the game</returns>
        public static string ThanksAndGoodbye()
        {
            return "You have managed to get out of your room. You have beated the game!\n" +
                "I hope you enjoyed playing this as much as I enjoyed creating it.\n" +
                "If you feel like giving feedback, don't hesitate to write me a PM on either Reddit or GitHub (name OuterGazer)\n\n" +
                "Designed, coded and written by OuterGazer on December 2020\n\n" +
                "Thank you for playing and goodbye!\n" +
                "Press Enter to continue...";
        }
    }
}