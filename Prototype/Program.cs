namespace PROTOTYPE;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. RPN Calculator");
            Console.WriteLine("2. Dungeon Crawlers");
            Console.WriteLine("3. Instructions");
            Console.WriteLine("Esc: Exit");
            ConsoleKey choice = Console.ReadKey().Key;
            if (choice == ConsoleKey.Escape) break;

            if (choice == ConsoleKey.D1)
            {
                RPNCalculator();
            }
            else if (choice == ConsoleKey.D2)
            {
                DungeonCrawlers();
            }
            else if (choice == ConsoleKey.D3)
            {
                Instructions();
            }
        }
    }

    static void Instructions()
    {
        Console.Clear();
        Console.WriteLine("RPN CALCULATOR:");
        Console.WriteLine("Infix expressions are the standard way of writing expressions");
        Console.WriteLine("Enter every part of the expression with a space in between");
        Console.WriteLine("Example: 3 * ( 10 + 4 )");

        Console.WriteLine("Postfix expressions (AKA Reverse Polish Notation) are a different way of writing expressions, where the operatoes follow their operands");
        Console.WriteLine("As with expresions, input every part with a space in between");
        Console.WriteLine("Example: 3 10 4 + *");

        Console.WriteLine();
        Console.WriteLine("DUNGEON CRAWLERS:");
        Console.WriteLine("Use the WASD keys to move one \"cell\" at a time");
        Console.WriteLine("Use the arrow keys to move all the way down a \"corridoor\" until you hit a wall");
        Console.WriteLine("Your goal is to obtain the key, noted by a blue \"+-\", and unlock the blue door to allow you to reach the exit");
        Console.WriteLine("You will start somewhere on the left hand side, your skin will be a \"[]\", the exit will be somewhere on the right hand side");

        Console.WriteLine();
        Console.WriteLine("Press any key to go back");
        Console.ReadKey();
    }

    static void RPNCalculator()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Infix to Postfix");
            Console.WriteLine("2. Postifx to Infix");
            Console.WriteLine("3. Evaluate Postfix");
            Console.WriteLine("Esc: Back");
            ConsoleKey choice = Console.ReadKey(true).Key;
            if (choice == ConsoleKey.Escape) break;

            if (choice == ConsoleKey.D1)
            {
                InfixToPostfix();
            }
            else if (choice == ConsoleKey.D2)
            {
                PostfixToInfix();
            }
            else if (choice == ConsoleKey.D3)
            {
                EvaluatePostfix();
            }
        }
    }
    static void InfixToPostfix()
    {
        while (true)
        {
            Stack<string> stack = new Stack<string>();
            List<string> postfix = new List<string>();

            Console.Clear();
            Console.WriteLine("INFIX TO POSTFIX");
            Console.Write("Input (include spaces between every operator and operand): ");
            string[] input = Console.ReadLine()!.Trim().Split(" ");
            Dictionary<string, int> precedence = new()
            {
                { "+", 0 },
                { "-", 0 },
                { "*", 1 },
                { "/", 1 },
            };
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    string token = input[i];

                    if (!"+-*/()".Contains(token))
                    {
                        postfix.Add(token);
                    }
                    else if (token == "(")
                    {
                        stack.Push(token);
                    }
                    else if (token == ")")
                    {
                        while (stack.Peek() != "(")
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Pop();
                    }
                    else
                    {
                        if (i - 1 == input.Length)
                        {
                            throw new Exception();
                        }
                        while (stack.Count > 0 && stack.Peek() != "(" && precedence[stack.Peek()] >= precedence[token])
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                }
                while (stack.Count > 0)
                {
                    postfix.Add(stack.Pop());
                }

                Console.Write("Output: ");
                foreach (string s in postfix)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                break;
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
    }
    static void PostfixToInfix()
    {
        while (true)
        {
            Stack<string> stack = new();

            Console.Clear();
            Console.WriteLine("POSTFIX TO INFIX");
            Console.Write("Input (include spaces between every operator and operand): ");
            string[] input = Console.ReadLine()!.Split(" ");
            Dictionary<string, int> precedence = new()
            {
                { "+", 0 },
                { "-", 0 },
                { "*", 1 },
                { "/", 1 }
            };

            try
            {
                foreach (string token in input)
                {
                    if (!"+-*/".Contains(token))
                    {
                        stack.Push(token);
                    }

                    else
                    {
                        string num1 = stack.Pop();
                        string num2 = stack.Pop();
                        string newExpression = $"({num2} {token} {num1})";
                        stack.Push(newExpression);
                    }
                }
                Console.WriteLine($"Output: {stack.Pop()}");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                break;
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
    }
    static void EvaluatePostfix()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("EVALUATING POSTFIX");
            Console.Write("Input (include a space between each operator/operand): ");
            string[] input = Console.ReadLine()!.Split(" ");
            Stack<string> stack = new Stack<string>();
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    string token = input[i];
                    if (!"+-*/".Contains(token))
                    {
                        stack.Push(token);
                    }

                    else
                    {
                        double num1 = int.Parse(stack.Pop());
                        double num2 = int.Parse(stack.Pop());
                        double outcome = 0;
                        char operation = char.Parse(token);
                        switch (operation)
                        {
                            case '+':
                                outcome = num2 + num1;
                                break;
                            case '-':
                                outcome = num2 - num1;
                                break;
                            case '*':
                                outcome = num2 * num1;
                                break;
                            case '/':
                                outcome = num2 / num1;
                                break;
                        }
                        stack.Push(outcome.ToString());
                    }
                }
                Console.WriteLine($"Output: {stack.Pop()}");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                break;
            }
            catch
            {
                Console.WriteLine("Invalid input");
                Console.WriteLine("Press any key to try again");
                Console.ReadKey();
            }
        }
    }

    static void DungeonCrawlers()
    {
        Console.Clear();
        Console.WriteLine("Please fullscreen before progressing. Press any key to continue");
        Console.ReadKey();
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.WriteLine("4. Tricky");
            Console.WriteLine("Esc: Back");

            ConsoleKey difficulty = Console.ReadKey(true).Key;
            if (difficulty == ConsoleKey.Escape) break;

            Dungeon? dungeon = null;

            if (difficulty == ConsoleKey.D1)
                dungeon = new(20, 20);
            else if (difficulty == ConsoleKey.D2)
                dungeon = new(40, 20);
            else if (difficulty == ConsoleKey.D3)
            {
                dungeon = new(40, 30);
            }
            else if (difficulty == ConsoleKey.D4)
            {
                dungeon = new(60, 50);
            }
            dungeon?.Play("[]");
        }
    }
}
