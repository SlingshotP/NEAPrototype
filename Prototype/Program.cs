namespace PROTOTYPE;

internal class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        Console.WriteLine("1. RPN Calculator");
        Console.WriteLine("2. Dungeon Crawlers");
        ConsoleKey choice = Console.ReadKey().Key;
        if (choice == ConsoleKey.D1)
        {
            RPNCalculator();
        }
        if (choice == ConsoleKey.D2)
        {
            DungeonCrawlers();
        }
    }

    static void RPNCalculator()
    {
        ConsoleKey choice = ConsoleKey.D0;
        while (choice != ConsoleKey.D4)
        {
            Console.Clear();
            Console.WriteLine("1. Infix to Postfix");
            Console.WriteLine("2. Postifx to Infix");
            Console.WriteLine("3. Evaluate Postfix");
            Console.WriteLine("4. Back");
            choice = Console.ReadKey(true).Key;

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
        Console.Clear();
        Console.WriteLine("INFIX TO POSTFIX");
        Console.Write("Input (include spaces between every operator and operand): ");
        string[] input = Console.ReadLine()!.Split(" ");
        Dictionary<string, int> precedence = new()
        {
            { "+", 0 },
            { "-", 0 },
            { "*", 1 },
            { "/", 1 },
        };

        Stack<string> stack = new Stack<string>();
        List<string> postfix = new List<string>();

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
    }
    static void PostfixToInfix()
    {
        Console.Clear();
        Console.WriteLine("POSTFIX TO INFIX");
        Console.Write("Input (include spaces between every operator and operand): ");
        string[] input = Console.ReadLine()!.Split(" ");
        Dictionary<string, int> precedence = new()
        {
            { "+", 0 },
            { "-", 0 },
            { "*", 1 },
            { "/", 1 },
        };

        Stack<string> stack = new();

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
    }


    static void EvaluatePostfix()
    {
        Console.Clear();
        Console.WriteLine("EVALUATING POSTFIX");
        Console.Write("Input (include a space between each operator/operand): ");
        string[] input = Console.ReadLine()!.Split(" ");
        Stack<string> stack = new Stack<string>();
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
            Console.WriteLine("Press escape to go back");

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
