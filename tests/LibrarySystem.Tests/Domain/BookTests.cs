using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class BookTests
{
    [TestMethod]
    public void Constructor_StoresBookDetails()
    {
        var book = new Book("Война и мир", "Лев Толстой", 1869);

        Assert.AreEqual("Война и мир", book.Title);
        Assert.AreEqual("Лев Толстой", book.Author);
        Assert.AreEqual(1869, book.Year);
    }

    [TestMethod]
    public void Constructor_MakesBookAvailable()
    {
        var book = new Book("Война и мир", "Лев Толстой", 1869);

        Assert.IsTrue(book.IsAvailable);
    }
}
