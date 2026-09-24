using LibrarySystem;

namespace LibrarySystem.Cli;

public sealed class ConsoleLoanEventHandler
{
    private readonly TextWriter _output;

    public ConsoleLoanEventHandler(TextWriter output)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    public void OnBookBorrowed(object? sender, LoanEventArgs args)
    {
        _output.WriteLine($"{args.Reader.Name} взял книгу '{args.Book.Title}'");
    }

    public void OnBookReturned(object? sender, LoanEventArgs args)
    {
        _output.WriteLine($"{args.Reader.Name} вернул книгу '{args.Book.Title}'");
    }
}
