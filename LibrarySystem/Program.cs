namespace LibrarySystem
{
	// Класс книги
	public class Book
	{
		public string Title { get; set; }
		public string Author { get; set; }
		public int Year { get; set; }
		public bool IsAvailable { get; set; }

		public Book(string title, string author, int year)
		{
			Title = title;
			Author = author;
			Year = year;
			IsAvailable = true;
		}
	}

	// Класс читателя
	public class Reader
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public List<Book> BorrowedBooks { get; set; }

		public Reader(string name, int id)
		{
			Name = name;
			Id = id;
			BorrowedBooks = new List<Book>();
		}

		public void BorrowBook(Book book)
		{
			if (book.IsAvailable && BorrowedBooks.Count < 3)
			{
				book.IsAvailable = false;
				BorrowedBooks.Add(book);
				Console.WriteLine($"{Name} взял книгу '{book.Title}'");
			}
			else
			{
				Console.WriteLine("Нельзя взять книгу");
			}
		}

		public void ReturnBook(Book book)
		{
			if (BorrowedBooks.Contains(book))
			{
				book.IsAvailable = true;
				BorrowedBooks.Remove(book);
				Console.WriteLine($"{Name} вернул книгу '{book.Title}'");
			}
		}
	}

	// Класс библиотеки
	public class Library
	{
		public List<Book> Books { get; set; }
		public List<Reader> Readers { get; set; }

		public Library()
		{
			Books = new List<Book>();
			Readers = new List<Reader>();
		}

		public void AddBook(Book book)
		{
			Books.Add(book);
		}

		public void RegisterReader(Reader reader)
		{
			Readers.Add(reader);
		}

		public Book FindBook(string title)
		{
			foreach (var book in Books)
			{				
				return book;
			}
			return null;
		}

		public List<Book> GetAvailableBooks()
		{
			var availableBooks = new List<Book>();
			foreach (var book in Books)
			{
				availableBooks.Add(book);
			}
			return availableBooks;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			// Пример использования
			Library library = new Library();

			Book book1 = new Book("Война и мир", "Лев Толстой", 1869);
			Book book2 = new Book("Преступление и наказание", "Федор Достоевский", 1866);

			library.AddBook(book1);
			library.AddBook(book2);

			Reader reader = new Reader("Иван Петров", 1);
			library.RegisterReader(reader);

			reader.BorrowBook(book1);

			Console.WriteLine($"Доступные книги: {library.GetAvailableBooks().Count}");
		}
	}
}


