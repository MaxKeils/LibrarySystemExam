namespace LibrarySystem;

public delegate bool BorrowingPolicy(Reader reader, Book book, int activeLoanCount);
