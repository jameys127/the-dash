using Spectre.Console;
public class Navigation
{
    public static ConsoleKeyInfo MenuNavigation()
    {
        ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
        if (Console.KeyAvailable)
        {
            pressedKey = Console.ReadKey(true);
        }
        return pressedKey;
    }
}