using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class LoanEventTests
{
    [TestMethod]
    public void BorrowAndReturn_RaiseEventsAfterStateChanges()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var book = new Book("Книга", "Автор", 2020);
        library.RegisterReader(reader);
        library.AddBook(book);

        var borrowedEvents = 0;
        var returnedEvents = 0;

        library.BookBorrowed += (sender, args) =>
        {
            borrowedEvents++;
            Assert.AreSame(library, sender);
            Assert.AreSame(book, args.Book);
            Assert.AreSame(reader, args.Reader);
            Assert.AreSame(reader, library.GetBorrower(book));
            Assert.IsFalse(book.IsAvailable);
        };

        library.BookReturned += (sender, args) =>
        {
            returnedEvents++;
            Assert.AreSame(library, sender);
            Assert.AreSame(book, args.Book);
            Assert.AreSame(reader, args.Reader);
            Assert.IsNull(library.GetBorrower(book));
            Assert.IsTrue(book.IsAvailable);
        };

        Assert.IsTrue(library.BorrowBook(reader, book));
        Assert.IsTrue(library.ReturnBook(reader, book));
        Assert.AreEqual(1, borrowedEvents);
        Assert.AreEqual(1, returnedEvents);
    }

    [TestMethod]
    public void RejectedOperations_DoNotRaiseEvents()
    {
        var library = new Library();
        var reader = new Reader("Иван", 1);
        var otherReader = new Reader("Мария", 2);
        var book = new Book("Книга", "Автор", 2020);
        library.AddBook(book);

        var eventCount = 0;
        library.BookBorrowed += (_, _) => eventCount++;
        library.BookReturned += (_, _) => eventCount++;

        Assert.IsFalse(library.BorrowBook(reader, book));
        library.RegisterReader(reader);
        library.RegisterReader(otherReader);
        Assert.IsTrue(library.BorrowBook(reader, book));
        Assert.IsFalse(library.BorrowBook(otherReader, book));
        Assert.IsFalse(library.ReturnBook(otherReader, book));
        Assert.IsTrue(library.ReturnBook(reader, book));
        Assert.IsFalse(library.ReturnBook(reader, book));
        Assert.AreEqual(2, eventCount);
    }
}
