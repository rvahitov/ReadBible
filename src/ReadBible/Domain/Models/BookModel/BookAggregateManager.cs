using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookAggregateManager
        : AggregateManager<BookAggregate, BookId, Command<BookAggregate, BookId>>
    {
    }
}