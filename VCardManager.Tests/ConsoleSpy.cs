using VCardManager.Core;

namespace VCardManager.Tests;

public class ConsoleSpy : IConsole
{
    private readonly Queue<string> inputs = new();
    public List<string> Output = new();

    public void Write(string message) => Output.Add(message);
    public void WriteLine(string message) => Output.Add(message);
    public string ReadLine()
    {
        if (inputs.Count > 0)
            return inputs.Dequeue();

        return "exit";
    }
    public void AddInput(string input) => inputs.Enqueue(input);
}