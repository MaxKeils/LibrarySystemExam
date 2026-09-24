# LibrarySystem

Небольшая библиотечная система на .NET 10. Доменная логика отделена от консольного примера: библиотека хранит книги, читателей и сведения о выдачах. Данные существуют только в памяти процесса.

## Возможности

- Добавление книг и регистрация читателей.
- Поиск книги по точному названию и просмотр доступных книг.
- Выдача и возврат книг с учётом текущего читателя.
- Ограничение: один читатель может держать не более трёх книг одновременно.

## Быстрый старт

Нужен .NET SDK 10.

```bash
dotnet restore LibrarySystem.sln -m:1
dotnet build LibrarySystem.sln --no-restore -m:1
dotnet run --project src/LibrarySystem.Cli/LibrarySystem.Cli.csproj
```

Консольный проект показывает пример выдачи. После выдачи одной из двух книг он выводит `Доступные книги: 1`.

## Структура

```text
src/
  LibrarySystem/
    Domain/               Книги, читатели и правила выдачи
  LibrarySystem.Cli/       Консольный пример
tests/
  LibrarySystem.Tests/
    Domain/               Модульные тесты MSTest
docs/
  assignment.md           Исходное учебное задание
.github/workflows/
  ci.yml                  Сборка и тесты на GitHub Actions
```

## Работа с библиотекой

```csharp
using LibrarySystem;

var library = new Library();
var book = new Book("Война и мир", "Лев Толстой", 1869);
var reader = new Reader("Иван Петров", 1);

library.AddBook(book);
library.RegisterReader(reader);

bool borrowed = library.BorrowBook(reader, book);
Reader? currentBorrower = library.GetBorrower(book);
bool returned = library.ReturnBook(reader, book);
```

`BorrowBook` возвращает `false`, если читатель не зарегистрирован, книги нет в фонде, она уже выдана или читатель достиг лимита. `ReturnBook` возвращает `false`, если книгу пытается вернуть не тот читатель. `GetBorrower` возвращает `null`, когда книга не выдана. Доступность `Book.IsAvailable` вычисляется по записям библиотеки.

Повторное добавление того же экземпляра книги и регистрация читателя с уже занятым `Id` вызывают `InvalidOperationException`. `FindBook` ищет по точному совпадению названия с учётом регистра и возвращает первую найденную книгу либо `null`.

## Тесты

```bash
dotnet test tests/LibrarySystem.Tests/LibrarySystem.Tests.csproj
```

Тесты проверяют создание книг и читателей, поиск, доступность, выдачу, возврат, лимит и принадлежность книги библиотеке. Те же проверки запускаются в GitHub Actions при отправке изменений.

## Ограничения

Это демонстрационный проект с хранением данных в памяти. После завершения процесса книги, читатели и выдачи не сохраняются. Консольный проект показывает фиксированный сценарий и не предоставляет интерактивный интерфейс.
