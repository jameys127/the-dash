using Spectre.Console;
public class Navigation
{
    public static ConsoleKey MenuNavigation()
    {
        ConsoleKey pressedKey = ConsoleKey.None; 
        if (Console.KeyAvailable)
        {
            pressedKey = Console.ReadKey(true).Key;
        }
        return pressedKey;
    }
}