using LibrarySystem;
using LibrarySystem.Cli;

var library = new Library();
var loanMessages = new ConsoleLoanEventHandler(Console.Out);
library.BookBorrowed += loanMessages.OnBookBorrowed;
library.BookReturned += loanMessages.OnBookReturned;

var book1 = new Book("Война и мир", "Лев Толстой", 1869);
var book2 = new Book("Преступление и наказание", "Федор Достоевский", 1866);

library.AddBook(book1);
library.AddBook(book2);

var reader = new Reader("Иван Петров", 1);
library.RegisterReader(reader);

library.BorrowBook(reader, book1);
Console.WriteLine($"Доступные книги: {library.GetAvailableBooks().Count}");

library.ReturnBook(reader, book1);
Console.WriteLine($"Доступные после возврата: {library.GetAvailableBooks().Count}");
