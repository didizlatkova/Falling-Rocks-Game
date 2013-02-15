using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FallingRocks
{
    struct Object
    {
        public int x;
        public int y;
        public ConsoleColor color;
        public string ch;
    }

    class FallingRocks
    {
        // Print elements on console
        static void PrintOnPosition(int x, int y,
            string ch, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(ch);
        }

        static void Main(string[] args)
        {
            int playfieldWidth = 50;
            int livesCount = 5;
            int speed = 50;
            int countSpeed = 50;
            int speedInfo = speed;

            // Remove scrollbar
            Console.BufferHeight = Console.WindowHeight = 30;
            Console.BufferWidth = Console.WindowWidth = 50;

            // Generate dwarf
            Object dwarf = new Object();
            dwarf.x = Console.WindowWidth/2-1;
            dwarf.y = Console.WindowHeight-1;
            dwarf.color = ConsoleColor.White;
            dwarf.ch = "(0)";
            Random randomGenerator = new Random();

            List<Object> stones = new List<Object>();
            List<ConsoleColor> colors = new List<ConsoleColor> {ConsoleColor.Blue,ConsoleColor.Cyan, ConsoleColor.DarkBlue, 
                ConsoleColor.Magenta,ConsoleColor.DarkMagenta,ConsoleColor.DarkGray, ConsoleColor.Yellow};

            List<string> chars = new List<string> { "&", "*", "?", "%", "$", "=", "+", "#" };
            while (true)
            {
                bool dwarfHit = false;
                bool winBonus = false;
                // Control speed increase
                countSpeed++;
                if (speed < 300)
                {
                    if (speed > 250)
                    {
                        if (countSpeed % 8 == 0)
                        {
                            speed++;
                        }
                    }                    
                    else if (speed > 200)
                    {
                        if (countSpeed % 6 == 0)
                        {
                            speed++;
                        }
                    }
                    else if (speed > 150)
                    {
                        if (countSpeed % 4 == 0)
                        {
                            speed++;
                        }
                    }
                    else if (speed > 100)
                    {
                        if (countSpeed % 2 == 0)
                        {
                            speed++;
                        }
                    }
                    else
                    {
                        speed++;
                    }
                }
                
                {
                    int chance = randomGenerator.Next(0, 100);
                    if (chance < 6)
                    {
                        if (chance < 3)
                        {
                            Object heart = new Object();
                            heart.color = ConsoleColor.Red;
                            heart.ch=((char)3).ToString();
                            heart.x = randomGenerator.Next(1, playfieldWidth-2);
                            heart.y = 6;
                            stones.Add(heart);
                        }
                        else
                        {
                            Object bonus = new Object();
                            bonus.color = ConsoleColor.Green;
                            bonus.ch=((char)6).ToString();
                            bonus.x = randomGenerator.Next(1, playfieldWidth-2);
                            bonus.y = 6;
                            stones.Add(bonus);
                        }

                    }
                    else
                    {
                        Object newStone = new Object();
                        newStone.color = colors[randomGenerator.Next(0, colors.Count)];
                        newStone.x = randomGenerator.Next(1, playfieldWidth-2);
                        newStone.y = 6;
                        newStone.ch = chars[randomGenerator.Next(0, chars.Count)];
                        stones.Add(newStone); 
                    }
                    // Generate heart

                    // Generate speed bonus

                    // Generate stone
                    
                }

                // Move dwarf (key pressed)
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    if (pressedKey.Key==ConsoleKey.LeftArrow)
                    {
                        if (dwarf.x - 1 > 0)
                        {
                            dwarf.x -= 1;   
                        }                        
                    }
                    else if (pressedKey.Key==ConsoleKey.RightArrow)
                    {
                        if (dwarf.x + 1 < playfieldWidth-3)
                        {
                            dwarf.x += 1;
                        }
                    }
                }                

                // Move stones
                List<Object> newStones = new List<Object>();
                 
                for (int i = 0; i < stones.Count; i++)
                {
                    Object oldStone = stones[i];
                    Object newStone = new Object();
                    newStone.x = oldStone.x;
                    newStone.y = oldStone.y + 1;
                    newStone.color = oldStone.color;
                    newStone.ch = oldStone.ch;

                    // Detect collision
                    if (newStone.y == dwarf.y && 
                        (newStone.x==dwarf.x||newStone.x==dwarf.x+1||newStone.x==dwarf.x+2))
                    {
                        if (newStone.ch == ((char)3).ToString())
                        {
                            livesCount++;
                            winBonus=true;
                        }
                        else if (newStone.ch == ((char)6).ToString())
                        {
                            speed -= 10 + speed%10;
                            winBonus = true;
                        }
                        else
                        {
                            livesCount--;
                            dwarfHit = true;
                        }
                       
                        // End game
                        if (livesCount == 0)
                        {
                            PrintOnPosition(1, 2, "Lives: " + livesCount.ToString(), ConsoleColor.White);
                            PrintOnPosition(20, 5, "GAME OVER!", ConsoleColor.DarkRed);
                            PrintOnPosition(18, 3, "Points: " + countSpeed.ToString(), ConsoleColor.Blue);
                            Console.SetCursorPosition(10,5);
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                    if (newStone.y < Console.WindowHeight)
                    {
                        newStones.Add(newStone);
                    } 
                }

                stones = newStones;                

                // Clear console
                Console.Clear();

                // Redraw playfield
                foreach (Object stone in stones)
                {
                    PrintOnPosition(stone.x, stone.y, stone.ch, stone.color);
                }
                if (dwarfHit)
                {
                    PrintOnPosition(dwarf.x, dwarf.y, "X", ConsoleColor.Red);
                    PrintOnPosition(dwarf.x + 1, dwarf.y, "X", ConsoleColor.Red);
                    PrintOnPosition(dwarf.x + 2, dwarf.y, "X", ConsoleColor.Red);
                }
                else if (winBonus)
                {
                    PrintOnPosition(dwarf.x, dwarf.y, "+", ConsoleColor.Green);
                    PrintOnPosition(dwarf.x + 1, dwarf.y, "+", ConsoleColor.Green);
                    PrintOnPosition(dwarf.x + 2, dwarf.y, "+", ConsoleColor.Green);
                }
                else
                {
                    PrintOnPosition(dwarf.x, dwarf.y, dwarf.ch, dwarf.color);
                }                

                // Display info
                    // Title
                    PrintOnPosition(18,1,"FALLING ROCKS",ConsoleColor.DarkRed);

                    // Lives
                    PrintOnPosition(1,2,"Lives: "+livesCount.ToString(),ConsoleColor.White);

                    // Speed
                    if (speed % 10 == 0)
                    {
                        speedInfo = speed;
                    }
                    PrintOnPosition(38, 2, "Speed: " + speedInfo.ToString(), ConsoleColor.White);

                // Slow down program
                Thread.Sleep(300-speed);
            }
        }
    }
}
