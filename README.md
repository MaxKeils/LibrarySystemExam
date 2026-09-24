# LibrarySystem

Небольшая библиотечная система на .NET 10. Библиотека хранит книги, читателей и сведения о выдачах. Правило выдачи передаётся делегатом, а успешные операции публикуются через события. Консольные сообщения обрабатываются отдельно от доменной логики. Данные существуют только в памяти процесса.

## Возможности

- Добавление книг и регистрация читателей.
- Поиск книги по точному названию и просмотр доступных книг.
- Выдача и возврат книг с учётом текущего читателя.
- По умолчанию один читатель может держать не более трёх книг одновременно; правило можно заменить.
- События выдачи и возврата для независимых обработчиков, например уведомлений или журнала.

## Быстрый старт

Нужен .NET SDK 10.

```bash
dotnet restore LibrarySystem.sln -m:1
dotnet build LibrarySystem.sln --no-restore -m:1
dotnet run --project src/LibrarySystem.Cli/LibrarySystem.Cli.csproj
```

Консольный проект показывает выдачу и возврат. После выдачи одной из двух книг он выводит `Доступные книги: 1`, а после возврата — `Доступные после возврата: 2`.

## Структура

```text
src/
  LibrarySystem/
    Domain/               Книги, читатели и учёт выдач
    Policies/             Делегат и готовые правила выдачи
    Events/               Данные событий выдачи и возврата
  LibrarySystem.Cli/
    Handlers/             Обработчик консольных сообщений
tests/
  LibrarySystem.Tests/
    Domain/               Тесты основных сценариев
    Policies/             Тесты правил выдачи
    Events/               Тесты событий
docs/
  assignment.md           Исходное учебное задание
.github/workflows/
  ci.yml                  Сборка и тесты на GitHub Actions
```

## Работа с библиотекой

```csharp
using LibrarySystem;

var library = new Library(BorrowingPolicies.MaxConcurrentBooks(2));
var book = new Book("Война и мир", "Лев Толстой", 1869);
var reader = new Reader("Иван Петров", 1);

library.BookBorrowed += (_, e) => Console.WriteLine($"{e.Reader.Name} взял {e.Book.Title}");
library.BookReturned += (_, e) => Console.WriteLine($"{e.Reader.Name} вернул {e.Book.Title}");

library.AddBook(book);
library.RegisterReader(reader);

bool borrowed = library.BorrowBook(reader, book);
Reader? currentBorrower = library.GetBorrower(book);
bool returned = library.ReturnBook(reader, book);
```

`BorrowBook` возвращает `false`, если читатель не зарегистрирован, книги нет в фонде, она уже выдана или правило выдачи отказало. `ReturnBook` возвращает `false`, если книгу пытается вернуть не тот читатель. `GetBorrower` возвращает `null`, когда книга не выдана. `Book.IsAvailable` изменяет только библиотека.

Делегат `BorrowingPolicy` получает читателя, книгу и число его текущих выдач. `BorrowingPolicies.MaxConcurrentBooks` создаёт правило с нужным лимитом; можно передать собственный делегат. `BookBorrowed` и `BookReturned` вызываются синхронно только после успешного изменения состояния. Обработчик консольных сообщений находится в `LibrarySystem.Cli` и подключается подпиской на эти события.

Повторное добавление того же экземпляра книги и регистрация читателя с уже занятым `Id` вызывают `InvalidOperationException`. `FindBook` ищет по точному совпадению названия с учётом регистра и возвращает первую найденную книгу либо `null`.

## Тесты

```bash
dotnet test tests/LibrarySystem.Tests/LibrarySystem.Tests.csproj
```

Тесты проверяют создание книг и читателей, поиск, доступность, выдачу, возврат, настраиваемые правила и события. Те же проверки запускаются в GitHub Actions при отправке изменений.

## Ограничения

Это демонстрационный проект с хранением данных в памяти. После завершения процесса книги, читатели и выдачи не сохраняются. Консольный проект показывает фиксированный сценарий и не предоставляет интерактивный интерфейс.
