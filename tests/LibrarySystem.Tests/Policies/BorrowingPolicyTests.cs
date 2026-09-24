using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class BorrowingPolicyTests
{
    [TestMethod]
    public void MaxConcurrentBooks_ChangesBorrowingLimit()
    {
        var library = new Library(BorrowingPolicies.MaxConcurrentBooks(1));
        var reader = new Reader("Иван", 1);
        var firstBook = new Book("Первая", "Автор", 2020);
        var secondBook = new Book("Вторая", "Автор", 2020);
        library.RegisterReader(reader);
        library.AddBook(firstBook);
        library.AddBook(secondBook);

        Assert.IsTrue(library.BorrowBook(reader, firstBook));
        Assert.IsFalse(library.BorrowBook(reader, secondBook));
        Assert.IsTrue(library.ReturnBook(reader, firstBook));
        Assert.IsTrue(library.BorrowBook(reader, secondBook));
    }

    [TestMethod]
    public void CustomDelegate_CanRejectBookByBusinessRule()
    {
        BorrowingPolicy recentBooksOnly = (_, book, _) => book.Year >= 2000;
        var library = new Library(recentBooksOnly);
        var reader = new Reader("Иван", 1);
        var oldBook = new Book("Старая", "Автор", 1990);
        var recentBook = new Book("Новая", "Автор", 2020);
        library.RegisterReader(reader);
        library.AddBook(oldBook);
        library.AddBook(recentBook);

        Assert.IsFalse(library.BorrowBook(reader, oldBook));
        Assert.IsTrue(library.BorrowBook(reader, recentBook));
        Assert.IsNull(library.GetBorrower(oldBook));
        Assert.AreSame(reader, library.GetBorrower(recentBook));
    }
}
