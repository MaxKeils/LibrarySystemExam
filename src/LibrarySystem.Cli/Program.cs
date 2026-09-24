using LibrarySystem;

var library = new Library();

var book1 = new Book("Война и мир", "Лев Толстой", 1869);
var book2 = new Book("Преступление и наказание", "Федор Достоевский", 1866);

library.AddBook(book1);
library.AddBook(book2);

var reader = new Reader("Иван Петров", 1);
library.RegisterReader(reader);

if (library.BorrowBook(reader, book1))
{
    Console.WriteLine($"{reader.Name} взял книгу '{book1.Title}'");
}

Console.WriteLine($"Доступные книги: {library.GetAvailableBooks().Count}");
