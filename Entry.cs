#define APP

// ReSharper disable once ClassNeverInstantiated.Global
public class Entry
{
    static void Main(string[] args)
    {
#if SAM
        var game = new LD54.App_Sam();

        game.Run();
#elif JAKUB
        var game = new LD54.App_Jakub();

        game.Run();
#else
        var game = new LD54.App();

        game.Run();
#endif
    }
}
