using LibrarySystem;

namespace LibrarySystem.Tests;

[TestClass]
public class ReaderTests
{
    [TestMethod]
    public void Constructor_StoresReaderDetails()
    {
        var reader = new Reader("Иван", 1);

        Assert.AreEqual("Иван", reader.Name);
        Assert.AreEqual(1, reader.Id);
    }
}
