using System;
using System.Collections;
using System.Collections.Generic;

namespace EscapeGame
{
    public class GameObject
    {
        /// <summary>
        /// Lists all possible parts within the room where the player can be.
        /// </summary>
        public enum RoomLocation
        {
            Center,
            Back,
            Entrance,
            Roof
        }

        /// <summary>
        /// To list if a certain item is open or closed. To use with windows, doors, drawers, etc.
        /// </summary>
        public enum ItemClosedOrOpen
        {
            IsClosed,
            IsOpen
        }

        /// <summary>
        /// To list if a certain item is on or off. To use with lamps, electronic devices, etc.
        /// </summary>
        public enum ItemOnOrOff
        {
            isOff,
            isOn
        }

        /// <summary>
        /// To list if certain items or objects have been combined, or used to a useful state that doesn't need to be reverted
        /// to a previous one.
        /// </summary>
        public enum ItemCombined
        {
            isNotCombined,
            isCombined,
        }

        //Variable to keep track of where the player is in the room
        public static RoomLocation playerPosition;

        //List that keeps track of the player's inventory
        public static List<string> playerInventory = new List<string>();

        //Array of ItemClosedOrOpen to keep the state of all items that can be opened or closed
        public static ItemClosedOrOpen[] isOpenable = new ItemClosedOrOpen[]
        {ItemClosedOrOpen.IsClosed, //0 - Window in Center
         ItemClosedOrOpen.IsClosed, //1 - Window in Back
         ItemClosedOrOpen.IsClosed  //2 - Wardrobe in Entrance
        };

        //Array of ItemOnOrOff to keep the state of all items that can be turned on or off
        public static ItemOnOrOff[] isSwitchable = new ItemOnOrOff[]
        {ItemOnOrOff.isOff, // 0 - lamp in center
         ItemOnOrOff.isOff  // 1 - lamp on desk in back
        };

        //Array of ItemCombined to keep the state of all items that can be combined and the state of key events
        public static ItemCombined[] isCombinable = new ItemCombined[]
        {ItemCombined.isNotCombined, //0 - Toy and batteries combined
         ItemCombined.isNotCombined, //1 - Combined Toy in shelf
         ItemCombined.isNotCombined, //2 - Desk drawers tidied up
         ItemCombined.isNotCombined, //3 - Key in desk drawer has showed up
         ItemCombined.isNotCombined, //4 - Key has been used in wardrobe
         ItemCombined.isNotCombined, //5 - Wardrobe examined for upper dark, unreachable spot
         ItemCombined.isNotCombined, //6 - Headlamp has/hasn't been discovered
         ItemCombined.isNotCombined, //7 - Batteries and headlamp combined
         ItemCombined.isNotCombined, //8 - Treasure Hunt Box has been found in upper part of wardrobe
         ItemCombined.isNotCombined, //9 - The riddle has been read
         ItemCombined.isNotCombined, //10 - Rope and carabiners have been picked
         ItemCombined.isNotCombined, //11 - Rope and carabiners have been combined
         ItemCombined.isNotCombined, //12 - Rope attached to outside ladder, roof accessible
         ItemCombined.isNotCombined, //13 - Box in chimney discovered with headlamp
         ItemCombined.isNotCombined, //14 - The box has been open and key discovered
         ItemCombined.isNotCombined, //15 - Victory state, player has left the room
         ItemCombined.isNotCombined, //16 - New sheets have been picked up from wardrobe
         ItemCombined.isNotCombined //17 - New sheets put in bed
        };

        //Arrays that list the objects in each part of the room
        public static string[] objectsInCenter = new string[]
        {"room", "bed", "window", "lamp", "ladder", "box", "pillow", "sheets", "kuro"};

        public static string[] objectsInBack = new string[]
        {"room", "shelf", "desk", "window", "toy", "batteries", "lamp", "drawers", "key", "headlamp", "books",
         "magazines", "figures", "gear"};

        public static string[] objectsInEntrance = new string[]
        {"room", "door", "wardrobe", "painting", "shoes", "clothes"};

        public static string[] objectsInRoof = new string[]
        {"roof", "chimney", "skylight", "tiles", "ground", "surroundings", "box"};

        /// <summary>
        /// Erases an item from the array of items so it can't be picked again.
        /// </summary>
        /// <param name="array">Array of objects from which to erase the particular item.</param>
        /// <param name="item">The item to be erased.</param>
        public static void EraseItem(string[] array, string item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == item)
                {
                    array.SetValue(null, i);
                    break;
                }
            }
        }

        /// <summary>
        /// To put back an item in an array so examine and use can work back again if an item previously on the
        /// inventory is put back again somewhere in the room.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="item"></param>
        public static void PutItemBack(string[] array, string item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    array.SetValue(item, i);
                    break;
                }
            }
        }

        /// <summary>
        /// This returns the description of items located in player's inventory.
        /// The description is usually more in depth as the one provided when the object was still unpicked.
        /// </summary>
        /// <param name="item">The item to be examined.</param>
        /// <returns>The description of the item</returns>
        public static string ItemDescriptionInventory(string item)
        {
            if(item == "toy")
            {
                if (isCombinable[0] == ItemCombined.isCombined)
                    return "Your old toy. Now you placed some batteries and it works.";
                
                return "Your old toy. For your surprise it still works after all this years but it doesn't have\n" +
                    "batteries right now. It needs 2 AAA batteries";
            }

            if (item == "batteries")
            {
                if ((isCombinable[7] == ItemCombined.isCombined) && (isCombinable[0] == ItemCombined.isCombined))
                    return "You have exactly one AAA battery left. You feel somewhat uneasy about the fact.";

                if ((isCombinable[7] == ItemCombined.isCombined) || (isCombinable[0] == ItemCombined.isCombined))
                    return "You have three AAA batteries left. You hope they are still charged.";

                return "A handful of AAA batteries. You wonder what they would be useful for.";
            }

            if (item == "key")
            {
                if (isCombinable[14] == ItemCombined.isCombined)
                    return "You inspect the key much closer. There is no doubt it is the key to your room. What's amazing is that it doesn't\n" +
                        "have the slightest stain of soot after all this time. That box sure was sealed tight.";

                //Because the wardrobe key and the door key will never be held by the player at the same time, this code works good enough
                if (isCombinable[3] == ItemCombined.isCombined)
                    return "It's a small key. You recognize it as the copy of your wardrobe key you did long ago.\n" +
                        "\"Why exactly did I do that?\" You ask yourself. But you don't entertain the thought much longer apart from\n" +
                        "thanking your old paranoid self from back then.";
            }

            if(item == "headlamp")
            {
                if (isCombinable[7] == ItemCombined.isNotCombined)
                    return "Your reliable headlamp. You feel it lighter than ususal. At closer examination you realize it has no batteries.";

                if (isCombinable[7] == ItemCombined.isCombined)
                    return "Your reliable headlamp. Its weight feels right now. It's ready for the next trip!";
            }

            if (item == "ladder")
                return "A small, albeit sturdy, aluminum folding ladder. Always handy!";

            if (item == "box")
            {
                if(isCombinable[13] == ItemCombined.isCombined)
                {
                    return "You clean some of the soot with your shirt. It's a little red box not much larger than your hand.\n" +
                        "What stands out to you is that you need a five number combination to open it... What could it be?";
                }
                else
                {
                    playerInventory.Remove("box");
                    playerInventory.Add("stack");
                    return "You eagerly open the box. Just as you suspected, is very carefully classified into \"solved\" and \n" +
                        "\"unsolved\" stacks. You wonder when exactly all that order obsession got lost in translation...\n" +
                        "You also control the urge of going down the nostalgia lane, discard the box to the side of the bed and\n" +
                        "just pick up the unsolved Stack for further examination";
                }                
            } 
            
            if(item == "stack")
            {
                playerInventory.Remove("stack");
                playerInventory.Add("riddle");
                return "You go through the rather thin stack of unsolved hunts. Your father was really good at it no matter\n" +
                    "how convoluted or abstract your clues were. You always wondered how he did it. Maybe you just were bad at it\n" +
                    "as your rate of solved hunts wasn't nearly as good but you feel glad you at least inherited his great sense\n" +
                    "for exploration. Anyhow, going randomly through the papers you lay your eyes in a single sheet titled \n" +
                    "\"The Ultimate Hunt\", you always had an eye for catchy titles, with some weird riddle on it.\n" +
                    "You put the stack back in the box and keep the riddle with you, because of the date you wrote on it proves\n" +
                    "it was the very last one you did. This has to be the one you are looking for.";
            }

            if (item == "riddle")
            {
                isCombinable[9] = ItemCombined.isCombined;
                return "You have a look at the riddle. It reads as follows:\n" +
                    "\"Sooting rain droplets\n" +
                      "Refreshing burning trees\n" +
                      "In summer forgotten\n" +
                      "Waiting for the CHI11.\"\n" +
                      "What did you mean by all this? Does it really make sense?";
            }

            if (item == "rope")
            {
                if(isCombinable[11] == ItemCombined.isCombined)
                {
                    return "The rope has a couple of bail-out carabiners attached at one of its ends. You suddenly feel like a ninja\n" +
                        "stealthing his way in the enemy fortress.";
                }
                else
                {
                    return "A barely five meter long dynamic climbing rope that you keep in your room for testing purposes.\n" +
                        "If it only was long enough... but the actual ropes you use for climbing and rappelling are all in the basement.";
                }
            }                

            if (item == "carabiner")
                return "Your strongest aluminum bail-out carabiners. Their wire gates makes it very easy to set them anywhere.";

            if (item == "note")
                return "You unfold the note and the first impression is quite strange, because the handwriting in it doesn't look like yours.\n" +
                    "You start reading it...\n" +
                    "\"Dear son, I hope this message reaches you in good time, and not that the box falls accidentally into the fire and burns\n" +
                    "everything down. That would be quite regretful and your mother would rightfully scold us to no end. Hopefully all that\n" +
                    "extra duct tape I put will help the single piece of it that you put to hold everything in place.\n" +
                    "But seriously, son... really? Sooting rain droplets refreshing burning trees? Like you thought for real I would never figure it out?\n" +
                    "I hope you improve your riddle writing in the future, or else you'll never become a good treasure hunter.\n" +
                    "Passwords made out of numbers with the position of letters in the alphabety are also pretty old...\n" +
                    "Let me know when you find this so we can celebrate with a bier. I know, I know. You aren't 18 just yet... let's keep this our" +
                    "little secret from your mother. I'd love to share all my knowledge with you.\"\n\n" +
                    "You are at a loss for words... You feel somewhat heartbroken.";

            if (item == "sheets")
            {
                if (isCombinable[17] == ItemCombined.isCombined)
                {
                    return "Another set of climbing themed sheets that you bought in a well known climbing brand. It's the living\n" +
                        "reminder that price not always translates to quality.";
                }
                else
                {
                    return "Your climbing themed sheets that you bought on a whim on that street market so long ago.\n" +
                    "You always get that outdoors nostalgic feel every time you look a them.";
                }
            }                

            if (item == "kuro")
                return "It's kuro! the little cuddly husky you gave your son for christmas when he was a little kid.\n" +
                    "He gave it back to you when he emancipated himself so it would remind you of him.\n" +
                    "He got his name for the black fur in his ears.";

            if (item == "book")
                return "A book with training exercises and programs for climbing. It is now outdated but it did help you break out of\n" +
                    "a nasty plateau back in the day and allowed you to climb your first 7a. And that makes it have a special place\n" +
                    "in your memories.";

            if (item == "magazine")
                return "A magazine about trail running in the Pyrenees. You always wanted to bring your hiking to the next level and this\n" +
                    "magazine seems to have some useful beginner advice.";

            if (item == "gear")
                return "A handful of nuts, a short dyneema cord with a prusik knot and a harness. You loose yourself for a moment\n" +
                    "remembering all the important climbing and speleology milestones that they were acive part of.";

            if(item == "shoes")
            {
                if (!playerInventory.Contains("rubber") && (isCombinable[11] == ItemCombined.isNotCombined))
                {
                    playerInventory.Add("rubber");
                    return "Your dirty and kind of smelly hiking shoes. They still stick to rock and brick like if they were glued to it.\n" +
                        "On closer examination you find some Rubber bands inside one of them. You pick them up, wondering why you put them there.";
                }
                else
                {
                    return "Your dirty and kind of smelly hiking shoes. They still stick to rock and brick like if they were glued to it.";
                }
            }

            if (item == "rubber")
                return "Some strong rubber bands. You have used them sometimes on your quickdraws for different purposes.\n" +
                    "Who put them in your hiking shoes?";

            if (item == "clothes")
                return "\"Keep Calm And Climb\". The slogan on your green hoody. Now you feel like hitting the crag. You have to get out!";

            return null;
        }

        /// <summary>
        /// This returns the description of items located in the center part of the room.
        /// </summary>
        /// <param name="item">The item to be examined.</param>
        /// <returns>The description of the item</returns>
        public static string ItemDescriptionCenter(string item)
        {
            if(item == "room")
            {
                Console.WriteLine("You look around the {0} part of the room", playerPosition.ToString());
                if (playerInventory.Contains("stack") || playerInventory.Contains("riddle"))
                {
                    return "There is your bed beneath the window. A lamp on the ceiling and the treasure hunting box resting\n" +
                        "next to the bed.";
                }
                else
                {
                    return "There is your bed beneath the window. A lamp on the ceiling.";
                }                
            }

            if(item == "bed")
            {
                if ((isCombinable[5] == ItemCombined.isNotCombined) && (!playerInventory.Contains("sheets")) &&
                    (!playerInventory.Contains("kuro")))
                {
                    if (isCombinable[17] == ItemCombined.isCombined)
                    {
                        return "Your newly done bed. It has your second favourite climbing themed sheets with your ergonomic pillow.\n" +
                            "Kuro, the cuddly husky that you once gifted your son, sits on the side and seems to be judgingly staring at you.";
                    }
                    else
                    {

                        return "Your undone bed. It has your favourite climbing themed sheets with your ergonomic pillow.\n" +
                            "Kuro, the cuddly husky that you once gifted your son, sits on the side and seems to be judgingly staring at you.";
                    }
                }

                if ((isCombinable[5] == ItemCombined.isNotCombined) && (playerInventory.Contains("sheets")) &&
                    (!playerInventory.Contains("kuro")))
                    return "Your bed without sheets. Only the white ergonomic pillow sadly lays there.\n" +
                        "Kuro, the cuddly husky that you once gifted your son, sits on the side and seems to be judgingly staring at you.";

                if ((isCombinable[5] == ItemCombined.isNotCombined) && (!playerInventory.Contains("sheets")) &&
                    (playerInventory.Contains("kuro")))
                {
                    if(isCombinable[17] == ItemCombined.isCombined)
                    {
                        return "Your newly done bed. It has your second favourite climbing themed sheets with your ergonomic pillow.\n";
                    }
                    else
                    {
                        return "Your undone bed. It has your favourite climbing themed sheets with your ergonomic pillow.\n";
                    }
                }                                    

                if ((isCombinable[5] == ItemCombined.isNotCombined) && (playerInventory.Contains("sheets")) &&
                    (playerInventory.Contains("kuro")))
                    return "Your bed without sheets. Only the white ergonomic pillow sadly lays there.";

                if ((isCombinable[5] == ItemCombined.isCombined) && (!playerInventory.Contains("ladder")))
                {
                    EraseItem(objectsInCenter, "ladder");
                    playerInventory.Add("ladder");
                    return "You remember you keep all your climbing gear stacked under the bed and decide to duck and look\n" +
                        "to see if there is anything useful. Sure enough, you see the little folding ladder you keep to reach\n" +
                        "the higher parts of the shelf and take it.";
                }

                if ((isCombinable[5] == ItemCombined.isCombined) && (playerInventory.Contains("ladder")))
                {
                    if((isCombinable[9] == ItemCombined.isCombined) && (isCombinable[10] == ItemCombined.isNotCombined))
                    {
                        playerInventory.Add("rope");
                        playerInventory.Add("carabiners");
                        isCombinable[10] = ItemCombined.isCombined;
                        return "You look under your bed, wondering for a bit. After a while you decide to pick up the rope and some carabiners";
                    }
                    else
                    {
                        return "You crouch and look under your bed again. There's all sorts of climbing related gear for both\n" +
                        "your speleology and climbing expeditions, but for now there isn't anything you might want to use.";
                    }
                }                    
            }

            if(item == "window")
            {
                if(isOpenable[0] == ItemClosedOrOpen.IsClosed)
                return "You look outside through the closed window. You only see the fields ahead of you";

                if (isOpenable[0] == ItemClosedOrOpen.IsOpen)
                    return "You get on your bed and lean through the opened window. The hopes of jumping to freedom\n" +
                        "are rather quickly smashed as you realize the three stories of free fall eagerly awaiting for you.\n" +
                        "To the right side you see the maintenance ladder that connects your room with the gable roof,\n" +
                        "but it's too far to reach it safely as there is no connecting ledge. Sometimes you wish you were taller.";
            }

            if(item == "lamp")
            {
                if(isSwitchable[0] == ItemOnOrOff.isOff)
                    return "The lamp is off.";

                if (isSwitchable[0] == ItemOnOrOff.isOn)
                    return "The lamp is on.";
            }

            if (item == "ladder")
                return "There is no such item here.";

            if(item == "box")
            {
                if(playerInventory.Contains("stack") || playerInventory.Contains("riddle"))
                {
                    return "The treasure hunt box with all the hunts. You'll have a look at it later when this is all over.";
                }
                else
                {
                    return "There is no such item here.";
                }
            }

            return null;
        }

        /// <summary>
        /// This returns the description of items located in the back part of the room.
        /// </summary>
        /// <param name="item">The item to be examined.</param>
        /// <returns>The description of the item</returns>
        public static string ItemDescriptionBack(string item)
        {
            if (item == "room")
            {
                Console.WriteLine("You look around the {0} part of the room", playerPosition.ToString());
                return "There is the shelf to the left and your work desk under the window.";
            }

            if(item == "shelf")
            {
                if(isCombinable[1] == ItemCombined.isCombined)
                {
                    return "Your shelf full of stuff. The toy is now where it should be.\n" +
                        "The upper shelf is filled with technical books and magazines about climbing, climbing training and\n" +
                        "speleology. The magazines cover a wide array of topics but they are mostly tourism marketing\n" +
                        "of different parts of the world.\n" +
                        "The second shelf is filled with your favourite books from other interests. Fiction, fantasy, history\n" +
                        "philosophy... The third shelf features different nerd collectibles that you used to put together with\n" +
                        "your son, like painted warhammer figures and videogame characters.\n" +
                        "The last shelf features old climbing gear that you don't use anymore until you find a better use for them.";
                }
                else
                {
                    return "Your shelf full of stuff. An empty base for some kind of toy sticks out to you.\n" +
                        "The lower shelf is filled with technical books and magazines about climbing, climbing training and\n" +
                        "speleology. The magazines cover a wide array of topics but they are mostly tourism marketing\n" +
                        "of different parts of the world.\n" +
                        "The second shelf is filled with your favourite books from other interests. Fiction, fantasy, history\n" +
                        "philosophy... The third shelf features different nerd collectibles that you used to put together with\n" +
                        "your son, like painted warhammer figures and videogame characters.\n" +
                        "The upmost shelf features old climbing gear that you don't use anymore until you find a better use for them.";
                } 
            }

            if (item == "books")
                return "You love to read, and this is your personal collection of favourite books that you have gathered through\n" +
                    "the years. Most are related to your job, but you also like to relax before bed by reading a bit of other kind of topics.\n" +
                    "That time of the year is getting close again, you want to buy more new books, but for that you will probably have to\n" +
                    "build a new shelf.";

            if (item == "magazines")
                return "Most of the magazines are about how pretty some places on earth are and how much you need to actually visit them.\n" +
                    "Some of them turned out to be good, but most were way too crowded. You still buy them because they are useful as starter\n" +
                    "points to find other interesting, less known places.\n" +
                    "It's not just climbing and cave exploration, they are also about hiking, camping and cycling.";

            if (item == "figures")
                return "Lots of expensive figures about videogame and role playing game characters. Some about your childhood characters\n" +
                    "but most have to do with characters familiar to your son. A good nostalgia collection that is actually worth quite\n" +
                    "some money. Sometimes you have thought about selling them online, but your son would kill you if you were to do that.";

            if (item == "gear")
                return "You keep around different kinds of old gear that you don't trust your life anymore. Every time you wonder why\n" +
                    "you don't recycle them so they don't use up space anymore you never arrive at a satisfying conclussion.";

            if(item == "desk")
            {
                if ((isCombinable[0] == ItemCombined.isCombined) && (isCombinable[5] == ItemCombined.isCombined))
                {
                    isCombinable[6] = ItemCombined.isCombined;
                    return "Among all your climbing gear you are still not done repairing, you lay your eyes on your speleology headlamp";
                }

                if ((isCombinable[5] == ItemCombined.isCombined) && (isCombinable[6] == ItemCombined.isNotCombined))
                {
                    isCombinable[6] = ItemCombined.isCombined;
                    return "Among all your climbing gear you are still not done repairing, you lay your eyes on your speleology headlamp";
                }


                if (!playerInventory.Contains("toy") && !playerInventory.Contains("batteries"))
                    return "Your favourite part of the house. Your reliable work desk. Untidy, with a lamp and a few drawers.\n" +
                        "There are various work tools and half repaired climbing objects scattered on it\n" +
                        "For now only a dinosaur toy and some batteries catch your attention.";

                if (!playerInventory.Contains("toy"))
                    return "Your favourite part of the house. Your reliable work desk. Untidy, with a lamp and a few drawers.\n" +
                        "There are various work tools and half repaired climbing objects scattered on it\n" +
                        "For now only a dinosaur toy catches your attention.";

                if (!playerInventory.Contains("batteries"))
                    return "Your favourite part of the house. Your reliable work desk. Untidy, with a lamp and a few drawers.\n" +
                        "There are various work tools and half repaired climbing objects scattered on it\n" +
                        "For now only some batteries catch your attention.";

                if (!playerInventory.Contains("toy") && playerInventory.Contains("batteries"))
                    return "Your favourite part of the house. Your reliable work desk. Untidy, with a lamp and a few drawers.\n" +
                        "There are various work tools and half repaired climbing objects scattered on it";

                return "Your favourite part of the house. Your reliable work desk. Untidy, with a lamp and a few drawers.\n" +
                    "There are various work tools and half repaired climbing objects scattered on it, nothing catches your eye.";
            }

            if(item == "toy")
            {
                if (isCombinable[1] == ItemCombined.isCombined)
                    return "Looking at your dinosaur toy on the shelf now, you really marvel how it has lasted for so long";
                
                return "An old dinosaur toy you liked to play with as a kid.";
            }

            if(item == "batteries")
            {
                return "just some regular AAA batteries.";
            }

            if(item == "drawers")
            {
                if ((isSwitchable[1] == ItemOnOrOff.isOff) && (isCombinable[2] == ItemCombined.isNotCombined))
                    return "The mess in your drawers even one-ups the mess on your desk and with the lamp off it looks even scary.";

                if ((isSwitchable[1] == ItemOnOrOff.isOn) && (isCombinable[2] == ItemCombined.isNotCombined))
                    return "The mess in your drawers even one-ups the mess on your desk. You realize that even better with the light on";

                if ((isSwitchable[1] == ItemOnOrOff.isOn) && (isCombinable[2] == ItemCombined.isCombined))
                {
                    isCombinable[3] = ItemCombined.isCombined;
                    return "You sort through your now tidier drawers. At first sight nothing catches your attention that\n" +
                        "could be useful right now. As you are about to close the last one, you sense a small, hard to see,\n" +
                        "switch at the front of the drawer. You push it. Almost instantly you hear a \"click\" noise and see\n" +
                        "a small key falling down to the floor from a small hidden compartment under the drawer";
                }                    

                if ((isSwitchable[1] == ItemOnOrOff.isOn) && (isCombinable[2] == ItemCombined.isCombined) &&
                    (isCombinable[3] == ItemCombined.isCombined))
                    return "Everything is a bit tidier now. You can comfortably look through your things but you don't see anything\n" +
                        "that could be useful right now.";
            }

            if(item == "headlamp")
            {
                if (isCombinable[6] == ItemCombined.isNotCombined)
                    return "There is no such item here";

                if (isCombinable[6] == ItemCombined.isCombined)
                    return "The headlamp's lightbulb got broken in one of your exploration trips, but you managed to replace it\n" +
                        "and it works well now. It rests on your desk where you left it when you were done with it \n" +
                        "as you never bothered putting it back into its place.";
            }

            if(item == "window")
            {
                if (isOpenable[1] == ItemClosedOrOpen.IsClosed)
                    return "You look outside through the closed window. You only see the fields ahead of you";

                if (isOpenable[1] == ItemClosedOrOpen.IsOpen)
                    return "You look outside through the opened window. Because of the desk you can't get close and only see\n" +
                        "the fields ahead of you. The cold breeze in your face reminds you of the summer all-nighters you used to\n" +
                        "pull out absorbed in your work.";
            }

            if (item == "lamp")
            {
                if (isSwitchable[1] == ItemOnOrOff.isOff)
                    return "The lamp on your desk is off.";

                if (isSwitchable[1] == ItemOnOrOff.isOn)
                    return "The lamp on your desk is on.";
            }

            if(item == "key")
            {
                if (isCombinable[3] == ItemCombined.isNotCombined)
                    return "There is no such item here";

                if (isCombinable[3] == ItemCombined.isCombined)
                    return "Bingo! The small key even seems to be eagerly looking at you.";
            }

            return null;
        }

        /// <summary>
        /// This returns the description of items located in the entrance part of the room.
        /// </summary>
        /// <param name="item">The item to be examined.</param>
        /// <returns>The description of the item</returns>
        public static string ItemDescriptionEntrance(string item)
        {
            if (item == "room")
            {
                Console.WriteLine("You look around the {0} part of the room", playerPosition.ToString());
                return "There is your room door, the embedded wardrobe at the opposite wall and a painting on the wall next\n" +
                    "to it.";
            }

            if(item == "door")
            {
                return "The door is locked!";
            }

            if(item == "wardrobe")
            {
                if (isCombinable[4] == ItemCombined.isCombined && (isOpenable[2] == ItemClosedOrOpen.IsOpen))
                {
                    if(isCombinable[8] == ItemCombined.isNotCombined)
                    {
                        isCombinable[5] = ItemCombined.isCombined;
                        return "You look through the wardrobe. Among all the hanging clothes you find nothing. You also search\n" +
                            "through the shoe drawers at the bottom but also find nothing. You look at the top, where you\n" +
                            "remember you keep all your boxes full with miscellaneous stuff. You can't reach it and it's also too dark\n" +
                            "and you don't want anything potentially falling on you and worsening your already bad headache.";
                    }
                    else
                    {
                        return "You look through the wardrobe. Among all the hanging clothes you find nothing. You also search\n" +
                            "through the shoe drawers at the bottom but also find nothing interesting.";
                    }                    
                }                    

                if ((isCombinable[4] == ItemCombined.isCombined) && (isOpenable[2] == ItemClosedOrOpen.IsClosed))
                    return "The wardrobe is now unlocked, but its doors are closed. You have always longed for X-Ray vision,\n" +
                        "but, alas! You weren't on the lucky side of the gene pool distribution, so you will need to manually\n" +
                        "open the doors.";

                return "The wardrobe is locked. That rascal Marvin must have also locked it before locking your room door!\n" +
                    "Although you can fuzzily remember you kept a spare key somewhere in your desk.";
            }

            if(item == "painting")
            {
                return "Kind of an ugly painting you did as a kid. You are still very proud of it.";
            }

            if(item == "shoes")
            {
                if (isCombinable[4] == ItemCombined.isCombined && (isOpenable[2] == ItemClosedOrOpen.IsOpen))
                {
                    return "You look through your shoes. You don't have a big collection, anyway. Your house shoes... your hiking shoes...\n" +
                    "your regular shoes... your flip-flops...";
                }
                else
                {
                    return "They are inside the wardrobe. You need to open it first.";
                }                   
            }

            if(item == "clothes")
            {
                if (isCombinable[4] == ItemCombined.isCombined && (isOpenable[2] == ItemClosedOrOpen.IsOpen))
                {
                    return "Your clean clothes. You spend a bit just feeling the smoothness of them through your fingers.";
                }
                else
                {
                    return "They are inside the wardrobe. You need to open it first.";
                }                    
            }

            return null;
        }

        /// <summary>
        ///This returns the description of items located in the roof.
        /// </summary>
        /// <param name="item">The item to be examined.</param>
        /// <returns>The description of the item<</returns>
        public static string ItemDescriptionRoof(string item)
        {
            if (item == "roof")
                return "You look around while standing on the roof. From there you have a nice view of the Surroundings and the Ground.\n" +
                    "The blue tiles still look nice after the last reparations that you did a few years ago. The chimney and the skylight\n" +
                    "to the guest room also catch your attention.";

            if (item == "chimney")
                return "The chimney looks normal from the outside. You take a peek inside but can't see much due to all the soot.\n" +
                    "it's been ages since it was last cleaned.";

            if(item == "box")
            {
                if(isCombinable[13] == ItemCombined.isCombined)
                {
                    return "You look a the box. The box looks back at you. What did you expect? For it to smile at you?";
                }
                else
                {
                    return "There is no such item here.";
                }
            }

            if (item == "skylight")
                return "The skylight is quite dusty, yet you can still see the guest room at the other side, where Marvin has been staying\n" +
                    "these days. You realize the door is open. You also make a mental note that he is not precisely the tidy kind of guy.";

            if (item == "tiles")
                return "shiny looking blue little things! They cost a fortune, too.";

            if (item == "ground")
                return "You examine he ground around the house. The backyard is empty and in the frontyard you can see Marvin's car parked\n" +
                    "next to Bob's and Margot's car. Are they also still in the house? You wonder why you haven't heard a single noise until now.";

            if (item == "surroundings")
                return "It's a sunny day. You can perfectly see your crops around the house and the few cows and sheep you still own chilling\n" +
                    "in the pastures.";


            return null;
        }

        /// <summary>
        /// This returns outcome of using an item located in the player's inventory with something else.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>The outcome of the use event</returns>
        public static string UseItemInventory(string item)
        {
            if (item == "box")
            {
                if (isCombinable[13] == ItemCombined.isCombined)
                {
                    Console.WriteLine("You try opening the box. You need to enter a five number combination...");
                    string combination = Console.ReadLine().Trim();

                    if(combination == "38911")
                    {
                        playerInventory.Add("note");
                        playerInventory.Add("key");
                        playerInventory.Remove("box");
                        isCombinable[14] = ItemCombined.isCombined;
                        return "As you enter the last digit you hear a sounding \"click\"... and the box opens before you!\n" +
                            "You look inside it and... YES! There is the spare key for your room! There is also a folded piece of paper.\n" +
                            "You take the note and the key and discard the box.";
                    }
                    else
                    {
                        return "Wrong combination it seems. The box won't open.";
                    }
                }
            }

            Console.WriteLine("What do you want to use the {0} with?", item);
            string itemToApply = Console.ReadLine().Trim().ToLower();

            if(item == "toy")
            {
                if(itemToApply == "batteries")
                {
                    //playerInventory.Remove("batteries");
                    isCombinable[0] = ItemCombined.isCombined;
                    return "You put 2 batteries into the toy. It now works.";
                }
                
                if(itemToApply == "shelf")
                {
                    if (playerPosition != RoomLocation.Back)
                        return "The shelf is not in this part of the room.";

                    if ((itemToApply == "shelf") && (isCombinable[0] == ItemCombined.isNotCombined))
                        return "There's a place for the toy in the shelf, but it must be functional first.";

                    if ((itemToApply == "shelf") && (isCombinable[0] == ItemCombined.isCombined))
                    {
                        playerInventory.Remove("toy");
                        PutItemBack(objectsInBack, "toy");
                        isCombinable[1] = ItemCombined.isCombined;
                        return "You place your toy where it belongs on the shelf.";
                    }
                }
            }

            if (item == "batteries")
            {
                if (itemToApply == "toy")
                {
                    //playerInventory.Remove("batteries");
                    isCombinable[0] = ItemCombined.isCombined;
                    return "You put 2 batteries into the toy. It now works.";
                }

                if(itemToApply == "headlamp")
                {
                    //playerInventory.Remove("batteries");
                    isCombinable[7] = ItemCombined.isCombined;
                    return "You put 2 batteries in the headlamp. It should work now.";
                }
            }

            if (item == "key")
            {
                if(isCombinable[14] == ItemCombined.isCombined)
                {
                    if(itemToApply == "door")
                    {
                        isCombinable[15] = ItemCombined.isCombined;
                        return "Who could have ever thought you forgot so much about your childhood? You wonder, as you fit the key into your\n" +
                            "room door's lock, lost in your thoughts. You open the door and stare at the dark hallway before you. You sigh for a moment,\n" +
                            "leave your room and start walking down the hallway." +
                            "Press Enter to continue...";
                    }
                }
                else
                {
                    if (itemToApply == "door")
                    {
                        return "Heh. You thought yourself smart for a moment but the key doesn't fit your room's door.\n" +
                            "You need to find the right key for it.";
                    }

                    if (itemToApply == "wardrobe")
                    {
                        playerInventory.Remove("key");
                        isCombinable[4] = ItemCombined.isCombined;
                        return "The key fits! You turn the key. The wardrobe is now unlocked.";
                    }
                }                
            }

            if (item == "headlamp")
            {                
                if (itemToApply == "batteries")
                {
                    //playerInventory.Remove("batteries");
                    isCombinable[7] = ItemCombined.isCombined;
                    return "You put 2 batteries in the headlamp. It should work now.";
                }

                if(itemToApply == "wardrobe")
                {
                    if(isCombinable[8] == ItemCombined.isNotCombined)
                    {
                        if (isOpenable[2] == ItemClosedOrOpen.IsOpen)
                        {
                            if (playerInventory.Contains("ladder"))
                            {
                                if (isCombinable[7] == ItemCombined.isCombined)
                                {
                                    playerInventory.Add("box");
                                    isCombinable[8] = ItemCombined.isCombined;
                                    return "You put on your headlamp and carefully place the folding ladder in front of the wardrobe.\n" +
                                        "You go up and examine the upper part... Among all the shoe boxes containing recipts, old photos\n" +
                                        "and even college books you didn't even remember that you had, you find a long forgotten box at\n" +
                                        "the very back labeled \"Treasure Hunts\". \"Isn't that convenient?\" You think to yourself.\n" +
                                        "You were quite skeptical at the beginning because memories have misled you in the past. But you\n" +
                                        "are now strongly believing you might have hidden a spare key somewhere in your room.\n" +
                                        "You put all the boxes back in their place and take the treasure hunt Box with you.";
                                }
                                else
                                {
                                    return "You can reach up there but the headlamp doesn't work.";
                                }
                            }
                            else
                            {
                                if (isCombinable[7] == ItemCombined.isCombined)
                                    return "You can see shoe boxes with different labels at the top of the wardrobe, but you can't\n" +
                                        "reach them.";

                                return "The headlamp isn't working. You can't see nor reach anything at the upper part of the wardrobe.";
                            }
                        }
                        else
                        {
                            return "You just might want to open the wardrobe first, don't you think?";
                        }
                    }
                    else
                    {
                        return "There is nothing else of interest for you in the upper part of the wardrobe;";
                    }
                }

                if(itemToApply == "chimney")
                {
                    if (playerPosition == RoomLocation.Roof)
                    {
                        isCombinable[13] = ItemCombined.isCombined;
                        return "You put on your headlamp and peek inside the chimney... There seems to be something attached to the chimney's\n" +
                            "inner wall... yeah! it looks like a box bound with duct tape! It's barely noticeable due to all the soot.\n" +
                            "It's also at arms reach.";
                    }                        
                }
            }
            
            if(item == "ladder")
            {
                if (itemToApply == "wardrobe")
                {
                    if(isCombinable[8] == ItemCombined.isNotCombined)
                    {
                        if (isOpenable[2] == ItemClosedOrOpen.IsOpen)
                        {
                            if (playerInventory.Contains("headlamp"))
                            {
                                if (isCombinable[7] == ItemCombined.isCombined)
                                {
                                    playerInventory.Add("box");
                                    isCombinable[8] = ItemCombined.isCombined;
                                    return "You put on your headlamp and carefully place the folding ladder in front of the wardrobe.\n" +
                                        "You go up and examine the upper part... Among all the shoe boxes containing recipts, old photos\n" +
                                        "and even college books you didn't even remember that you had, you find a long forgotten box at\n" +
                                        "the very back labeled \"Treasure Hunts\". \"Isn't that convenient?\" You think to yourself.\n" +
                                        "You were quite skeptical at the beginning because memories have misled you in the past. But you\n" +
                                        "are now strongly believing you might have hidden a spare key somewhere in your room.\n" +
                                        "You put all the boxes back in their place and take the treasure hunt Box with you.";
                                }
                                else
                                {
                                    return "You can reach up there but the headlamp doesn't work.";
                                }
                            }
                            else
                            {
                                return "You can reach up there but can't see anything.";
                            }
                        }
                        else
                        {
                            return "You just might want to open the wardrobe first, don't you think?";
                        }
                    }
                    else
                    {
                        return "There is nothing else of interest for you in the upper part of the wardrobe;";
                    }
                }

                if((itemToApply == "shelf") || (itemToApply == "gear"))
                {
                    if (!playerInventory.Contains("gear"))
                    {
                        playerInventory.Add("gear");
                        return "You put the ladder before the shelf and reach up to the uppest laying shelf.\n" +
                            "After looking for a while you pick up some gear that you think it could be useful.";
                    }
                    else
                    {
                        return "You don't need anymore gear at the moment, so you leave it alone.";
                    }                    
                }
                    
            }

            if(item == "rope")
            {
                if(itemToApply == "carabiners")
                {
                    if (playerInventory.Contains("rubber"))
                    {
                        playerInventory.Remove("carabiners");
                        playerInventory.Remove("rubber");
                        isCombinable[11] = ItemCombined.isCombined;
                        return "With excitement you attach two carabiners at the end of the rope with a safety knot.\n" +
                            "You use the rubber to keep the gates opened.";
                    }
                    else
                    {
                        return "That seems like a good idea. But you need a way to keep the gates of the carabiners open.";
                    }                    
                }

                if((itemToApply == "window") && (playerPosition == RoomLocation.Center))
                {
                    if(isOpenable[0] == ItemClosedOrOpen.IsOpen)
                    {
                        if (isCombinable[11] == ItemCombined.isCombined)
                        {
                            isCombinable[12] = ItemCombined.isCombined;
                            return "Good idea! Carefully watching your step you pull half your body outside of the window. You swing the rope\n" +
                                "with the attached carabiners a few times towards the ladder until you manage to clip them. You come into the room\n" +
                                "and tie the rope to the foot of the bed with a security know until is tight enough. You should be able to access\n" +
                                "the Roof now!";
                        }
                        else
                        {
                            return "You carefully lean out the window and try to use the rope as a whip with the ladder hoping it will stick.\n" +
                                "No luck, though.";
                        }
                    }
                    else
                    {
                        return "You feel very attached to the glass of your windows and don't wish to break them.\n" +
                            "It would be a better idea opening the window first";
                    }
                }

                if ((itemToApply == "lamp") && (playerPosition == RoomLocation.Center))
                    return "You feel a sudden rush for tarzaning your way through the room, but the envision of a broken hip brings you\n" +
                        "quickly into your senses again.";
                    
            }

            if (item == "carabiners")
            {
                if (itemToApply == "rope")
                {
                    if (playerInventory.Contains("rubber"))
                    {
                        playerInventory.Remove("carabiners");
                        playerInventory.Remove("rubber");
                        isCombinable[11] = ItemCombined.isCombined;
                        return "With excitement you attach two carabiners at the end of the rope with a safety knot.\n" +
                            "You use the rubber to keep the gates opened.";
                    }
                    else
                    {
                        return "That seems like a good idea. But you need a way to keep the gates of the carabiners open.";
                    }
                }

            }
            
            if(item == "sheets")
            {
                if(playerPosition == RoomLocation.Center)
                {
                    if (itemToApply == "window")
                        return "Doing knots with the sheets and climb down your way to freedom. It's always like that in the movies.\n" +
                            "Luckily for you, you are knowledgeable enough that the sheets would tear apart at the very moment that\n" +
                            "you would put your weight on them.";

                    if((itemToApply == "bed") && (isCombinable[16] == ItemCombined.isCombined))
                    {
                        playerInventory.Remove("sheets");
                        PutItemBack(objectsInCenter, "sheets");
                        isCombinable[17] = ItemCombined.isCombined;
                        return "You put the new sheets in the bed and do it properly. Mmm, such a fresh odor...";
                    }
                }
                else
                {
                    return "Maybe it works? You are not in the right part of the room, though.";
                }

                if (playerPosition == RoomLocation.Back)
                {
                    if (itemToApply == "window")
                        return "Doing knots with the sheets and climb down your way to freedom. You would have to move the desk out\n" +
                            "of the way for that, though. And the desk is heavy. Like really, really heavy...\n" +
                            "Nah, just kidding, it's not that heavy but you just don't feel like it with your current hangover.";
                }
                else
                {
                    return "Maybe it works? You are not in the right part of the room, though.";
                }

                if (playerPosition == RoomLocation.Entrance)
                {
                    if ((itemToApply == "wardrobe") && (isCombinable[4] == ItemCombined.isCombined) &&
                        (isOpenable[2] == ItemClosedOrOpen.IsOpen))
                    {
                        isCombinable[16] = ItemCombined.isCombined;
                        return "You search the wardrobe for some clean sheets. You find one that pleases you and substitute the dirty\n" +
                            "ones in your inventory for them.";
                    }
                    else
                    {
                        return "You can't look through a closed wardrobe.";
                    }                        
                }
                else
                {
                    return "The wardrobe is in a different part of the room.";
                }
            }

            if(item == "kuro")
            {
                if(itemToApply == "window")
                {
                    if((isOpenable[0] == ItemClosedOrOpen.IsClosed) && (isOpenable[1] == ItemClosedOrOpen.IsClosed))
                    {
                        return "You look at kuro's sad eyes and promptly throw him through one of the windows.\n" +
                            "Luckily they are closed and he bounces back into the room. You pick him back again.\n" +
                            "Now, why would you do something like that?";
                    }
                    else
                    {
                        playerInventory.Remove("kuro");
                        return "You look at kuro's sad eyes and promptly throw him through one of the opened windows.\n" +
                            "You inmediately regret doing that... Then you hear a cat shrieking. \"Bullseye!\" You think to yourself\n" +
                            "After a chuckle, you feel sad for Nestor. But he had it coming.\n" +
                            "You make a mental note of picking kuro back up and wash it once you get out of there.";
                    }
                }
            }

            if((item == "book") || (item == "magazine"))
            {
                if(itemToApply == "bed")
                {
                    return "Now it's not the time to be reading.";
                }

                if (itemToApply == "window")
                {
                    if ((isOpenable[0] == ItemClosedOrOpen.IsClosed) && (isOpenable[1] == ItemClosedOrOpen.IsClosed))
                    {
                        return "In a fit of rage due to the frustration of being trapped in our room you throw it through the window.\n" +
                            "The window is closed, though. And it also miraculously didn't break. You thank your luck and pick it back up.";
                    }
                    else
                    {
                        playerInventory.Remove("book");
                        playerInventory.Remove("magazine");
                        return "In a fit of rage due to the frustration of being trapped in our room you throw it through the window.\n" +
                            "You see it describe an arch into the void and loose sight of it. After a short while you hear a clunky sound...\n" +
                            "Did it fall on your car?!";
                    }
                }
            }

            return "That doesn't work.";
        }

        /// <summary>
        /// This returns the outcome of using an item located in the center part of the room.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>The outcome of the use event</returns>
        public static string UseItemCenter(string item)
        {            
            if (item == "bed")
            {
                return "You'd want to sleep a bit more, but only once you get out and eat something.";
            }

            if (item == "window")
            {
                if(isOpenable[0] == ItemClosedOrOpen.IsClosed)
                {
                    isOpenable[0] = ItemClosedOrOpen.IsOpen;
                    return "You open the window.";
                }

                if (isOpenable[0] == ItemClosedOrOpen.IsOpen)
                {
                    isOpenable[0] = ItemClosedOrOpen.IsClosed;
                    return "You close the window.";
                }

            }

            if (item == "lamp")
            {
                if (isSwitchable[0] == ItemOnOrOff.isOff)
                {
                    isSwitchable[0] = ItemOnOrOff.isOn;
                    return "You switch the ceiling lamp on.";
                }

                if (isSwitchable[0] == ItemOnOrOff.isOn)
                {
                    isSwitchable[0] = ItemOnOrOff.isOff;
                    return "You switch the ceiling lamp off.";
                }

            }

            return "You can't seem to find any use for that.";
        }

        /// <summary>
        /// This returns outcome of using an item located in the back part of the room.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>The outcome of the use event</returns>
        public static string UseItemBack(string item)
        {
            if (item == "shelf")
            {
                return "What would you want to do with it, anyway? Maybe it's just better to have a closer look at it.";
            }

            if (item == "desk")
            {
                return "There are so many things on your desk that you can't decide what to start playing around with.";
            }

            if (item == "window")
            {
                if (isOpenable[1] == ItemClosedOrOpen.IsClosed)
                {
                    isOpenable[1] = ItemClosedOrOpen.IsOpen;
                    return "You open the window.";
                }

                if (isOpenable[1] == ItemClosedOrOpen.IsOpen)
                {
                    isOpenable[1] = ItemClosedOrOpen.IsClosed;
                    return "You close the window.";
                }

            }

            if (item == "lamp")
            {
                if (isSwitchable[1] == ItemOnOrOff.isOff)
                {
                    isSwitchable[1] = ItemOnOrOff.isOn;
                    return "You switch the desk lamp on.";
                }

                if (isSwitchable[1] == ItemOnOrOff.isOn)
                {
                    isSwitchable[1] = ItemOnOrOff.isOff;
                    return "You switch the desk lamp off.";
                }

            }

            if (item == "drawers")
            {
                if ((isCombinable[2] == ItemCombined.isNotCombined) && (isSwitchable[1] == ItemOnOrOff.isOff))
                {
                    return "You feel like killing some time tyding up your drawers but there is not enough light to properly\n" +
                        "see through everything.";
                }
                if ((isCombinable[2] == ItemCombined.isNotCombined) && (isSwitchable[1] == ItemOnOrOff.isOn))
                {
                    isCombinable[2] = ItemCombined.isCombined;
                    return "You finally make up your mind and spend some time tyding up your desk drawers.\n" +
                        "After you are done you not only feel better about yourself, but you also realize that is now much\n" +
                        "easier to look through the stuff stored there.";
                }
                if (isCombinable[2] == ItemCombined.isCombined)
                    return "The drawers are good as they are now. there is no need to mess with them further.";
            }

            if (item == "key")
            {
                if (isCombinable[3] == ItemCombined.isNotCombined)
                    return "There is no such item here.";

                if (isCombinable[3] == ItemCombined.isCombined)
                    return "What do you want to use that for, there lying on the floor? Just pick it up.";
            }

            if (item == "headlamp")
            {
                if (isCombinable[3] == ItemCombined.isNotCombined)
                    return "There is no such item here";

                if (isCombinable[3] == ItemCombined.isCombined)
                    return "The headlamp is of no use to you just chilling there on the desk";
            }

            return "You can't seem to find any use for that.";
        }

        /// <summary>
        /// This returns outcome of using an item located in the entrance part of the room.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>The outcome of the use event</returns>
        public static string UseItemEntrance(string item)
        {
            if (item == "door")
            {
                return "It's locked, you can't open it.";
            }

            if (item == "wardrobe")
            {
                if ((isCombinable[4] == ItemCombined.isCombined) && (isOpenable[2] == ItemClosedOrOpen.IsClosed))
                {
                    isOpenable[2] = ItemClosedOrOpen.IsOpen;
                    return "You open the wardrobe.";
                }

                if ((isCombinable[4] == ItemCombined.isCombined) && (isOpenable[2] == ItemClosedOrOpen.IsOpen))
                {
                    isOpenable[2] = ItemClosedOrOpen.IsClosed;
                    return "You close the wardrobe.";
                }

                return "The wardrobe is locked. That rascal Marvin must have also locked it before locking your room door!\n" +
                    "Although you can fuzzily remember you kept a spare key somewhere in your desk.";
            }
            
            if (item == "painting")
            {
                return "You look behind it in hopes of a secret compartment... but no luck.";
            }

            return "You can't seem to find any use for that.";
        }

        /// <summary>
        ///This returns outcome of using an item located in the entrance part of the room.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <returns>The outcome of the event<</returns>
        public static string UseItemRoof(string item)
        {
            if (item == "roof")
                return "The grandiosity of using the entire roof for a purpose keeps you busy for a fraction of a second before you toss\n" +
                    "the idea to your mental garbage bin, where it should have never got out from.";

            if (item == "chimney")
                return "It's a hot summer day and you're long past your Santa Claus' complex days, my friend. Besides, you have no way of cleaning\n" +
                    "yourself if you get all covered with soot. Although that would be funny for a prank. You make a mental note of that.";

            if(item == "box")
            {
                if (isCombinable[13] == ItemCombined.isCombined)
                {
                    return "You die inside for using the box... Really! You are! That's why you acknowledge you should pick it up first.";
                }
                else
                {
                    return "There is no such item here.";
                }
            }

            if (item == "skylight")
                return "You try opening the skylight but it's locked from the inside. You also knock it for a while but there is no response.";

            if (item == "tiles")
                return "The tiles are good where they are. If your wife finds out you ruined those incredibly expensive little fellows\n" +
                    "you will never hear the end of it.";

            if (item == "ground")
                return "The thought of emulating your favourite action movies, jumping and rolling over a car roof to the ground,\n" +
                    "crosses your mind just enough time that you almost trip down to your certain death.\n" +
                    "Instead, you decide to yell your friends' names and call for help.\n" +
                    "After a while of nobody answering you decide to stop, wondering what in the world is wrong. \"Could they be really that mad at me?\"\n" +
                    "You wonder.";

            if (item == "surroundings")
                return "\"Use the surroundings to your advantage!\" It's an archetypical quote of many movies. You can't imagine what good\n" +
                    "can you make out of your poor cows and sheep in your situation, though.";


            return null;
        }

        /// <summary>
        /// This decides if an item located in the center part of the room can be picked or not and 
        /// adds it to the inventory if picked.
        /// </summary>
        /// <param name="item">The item to be picked.</param>
        /// <returns></returns>
        public static string PickItemCenter(string item)
        {
            if (item == "room")
            {                
                return "Sure, just let me get my magic hat that fits everything in it.";
            }

            if (item == "bed")
            {
                return "I'm afraid is just a tad bit too big for my pockets.";
            }

            if (item == "window")
            {
                return "Better to leave the window just where it is. It's serving its purpose rather well.";
            }

            if (item == "lamp")
            {
                return "I can't reach it, and even if I could... What would I do with it?.";
            }

            if(item == "sheets")
            {
                if(isCombinable[17] == ItemCombined.isNotCombined)
                {
                    EraseItem(objectsInCenter, "sheets");
                    playerInventory.Add("sheets");
                    return "With a bit of effort you undo the bed and pick up the sheets.";
                }
                else
                {
                    return "The bed is good now how it is. There is no need to take out the sheets again.";
                }                
            }

            if(item == "kuro")
            {
                EraseItem(objectsInCenter, "kuro");
                playerInventory.Add("kuro");
                return "You pick up kuro. You like the feel of him accompanying you through your endeavors.";
            }

            return null;
        }

        /// <summary>
        /// This decides if an item located in the back part of the room can be picked or not and 
        /// adds it to the inventory if picked.
        /// </summary>
        /// <param name="item">The item to be picked.</param>
        /// <returns></returns>
        public static string PickItemBack(string item)
        {
            if (item == "room")
            {                
                return "I'm sure that would cause a rip in the space continuum.";
            }

            if (item == "shelf")
            {
                return "The shelf is full of stuff. Better to just pick the individual items.";
            }

            if(item == "books")
            {
                if (!playerInventory.Contains("book"))
                {
                    playerInventory.Add("book");
                    return "You pick a random book from the shelf expecting a secret passage to open itself... nothing happens.";
                }
                else
                {
                    return "One book is enough. Hoarding is not the answer.";
                }                
            }

            if(item == "magazines")
            {
                if (!playerInventory.Contains("magazine"))
                {
                    playerInventory.Add("magazine");
                    return "You pick a magazine about the Pyrenees that you still haven't finished. You could kill some time with it.";
                }
                else
                {
                    return "So you really do want to pick absolutely everything that isn't nailed to the ground... Not under my watch!";
                }                
            }

            if (item == "figures")
                return "Better to leave them alone. They are expensive and fragile and your son has a remarkable selective memory\n" +
                    "when it comes to remember the state of all this figures.";

            if(item == "gear")
            {
                if (playerInventory.Contains("ladder"))
                {
                    if (!playerInventory.Contains("gear"))
                    {
                        playerInventory.Add("gear");
                        return "You put the ladder before the shelf and reach up to the uppest laying shelf.\n" +
                            "After looking for a while you pick up some gear that you think it could be useful.";
                    }
                    else
                    {
                        return "You don't need anymore gear at the moment, so you leave it alone.";
                    }
                }
                else
                {
                    return "The shelf with the gear is out of reach. You can't take anything from there.";
                }
            }

            if (item == "desk")
            {
                return "It's not like I can just grab it and walk around with it, can't I?";
            }

            if (item == "window")
            {
                return "Better to leave the window just where it is. It's serving its purpose rather well.";
            }

            if(item == "toy")
            {
                if (isCombinable[1] == ItemCombined.isCombined)
                    return "The toy is good where it is.";
                
                EraseItem(objectsInBack, "toy");

                playerInventory.Add("toy");

                return "You pick up the toy";
            }

            if (item == "batteries")
            {
                EraseItem(objectsInBack, "batteries");

                playerInventory.Add("batteries");

                return "You pick up the batteries";
            }

            if (item == "key")
            {
                if (isCombinable[3] == ItemCombined.isNotCombined)
                    return "There is no such item here.";

                if (isCombinable[3] == ItemCombined.isCombined)
                {
                    EraseItem(objectsInBack, "key");

                    playerInventory.Add("key");

                    return "You pick up the small key";
                }
            }

            if (item == "headlamp")
            {
                if (isCombinable[6] == ItemCombined.isNotCombined)
                    return "There is no such item here.";

                if (isCombinable[6] == ItemCombined.isCombined)
                {
                    EraseItem(objectsInBack, "headlamp");

                    playerInventory.Add("headlamp");

                    return "You pick up the headlamp";
                }
            }

            return null;
        }

        /// <summary>
        /// This decides if an item located in the entrance part of the room can be picked or not and 
        /// adds it to the inventory if picked.
        /// </summary>
        /// <param name="item">The item to be picked.</param>
        /// <returns></returns>
        public static string PickItemEntrance(string item)
        {
            if (item == "room")
            {
                return "I'm not quite sure what the thought process is here.";
            }

            if (item == "door")
            {
                return "It's well built into the wall. I can't tear it apart just like that.";
            }

            if (item == "wardrobe")
            {
                return "The wardrobe is embedded into the wall. I'm open to ideas on how to pick it.";
            }

            if (item == "painting")
            {
                return "No, It's too big to carry it around.";
            }

            if(item == "shoes")
            {
                if ((isCombinable[4] == ItemCombined.isNotCombined) || (isOpenable[2] == ItemClosedOrOpen.IsClosed))
                {
                    return "The wardrobe is not opened. You could hit your hands millions of times for that one chance they may go through...h\n" +
                        "but you'd rather open it first.";                    
                }else if (!playerInventory.Contains("shoes"))
                {
                    playerInventory.Add("shoes");
                    return "You pick up your hiking shoes for later.";
                }
                else
                {
                    return "You don't want to be carrying around more stinky shoes. One pair is enough to advertise your presence\n" +
                        "from kilometers already.";
                }                    
            }

            if (item == "clothes")
            {
                if ((isCombinable[4] == ItemCombined.isNotCombined) || (isOpenable[2] == ItemClosedOrOpen.IsClosed))
                {
                    return "The wardrobe is not opened. You could hit your hands millions of times for that one chance they may go through...h\n" +
                        "but you'd rather open it first.";
                }
                else if (!playerInventory.Contains("clothes"))
                {
                    playerInventory.Add("clothes");
                    return "You pick up your favourite hoody that you got as a consolation price in a climbing competition\n" +
                        "and put it on.";
                }
                else
                {
                    return "You are already warm enough. Imagine the embarrasement if you were to be found dead in your room due to a\n" +
                        "combination of suffocating from lack of air and dehydration for excessive sweating. You are not ready yet to be\n" +
                        "the world's laughing stock.";
                }
            }

            return null;
        }

        /// <summary>
        /// This decides if an item located in the roof can be picked or not and 
        /// adds it to the inventory if picked.
        /// </summary>
        /// <param name="item">The item to be picked.</param>
        /// <returns></returns>
        public static string PickItemRoof(string item)
        {
            if (item == "roof")
                return "Hmm, sounds like a good idea on paper. And we are on a screen, so imagine.";

            if (item == "chimney")
                return "The chimney is well attached to the rest of the roof. It's not going anywhere any time soon.";

            if(item == "box")
            {
                if(isCombinable[13] == ItemCombined.isCombined)
                {
                    playerInventory.Add("box");
                    EraseItem(objectsInRoof, "box");
                    return "You reach down to the box. You sure did put a lot of duct tape back then... after a bit of a struggle you manage\n" +
                        "to tear it from the wall. That's it! This has to be what that riddle was all about!";
                }
                else
                {
                    return "There is no such item here.";
                }
            }

            if (item == "skylight")
                return "The skyight is locked from the inside, so you can't pick it if you can't even open it.";

            if (item == "tiles")
                return "Good idea! You pick some tiles carefully not to damage them. Your idea is to squeeze yourself through the wood structure...\n" +
                    "however you didn't account for the various insulation and gypsum plasterboard layers underneath... You are not that desperate\n" +
                    "just yet that you would tear down your house so you put the tiles back in their place.";

            if (item == "ground")
                return "I mean, I consider myself a \"down to earth\" kind of guy, but this is taking it too far, don't you think?";

            if (item == "surroundings")
                return "You are starting to really scare me with your ideas.";


            return null;
        }
    }
}