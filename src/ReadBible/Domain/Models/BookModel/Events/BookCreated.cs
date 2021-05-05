using Akkatecture.Aggregates;

namespace ReadBible.Domain.Models.BookModel.Events
{
    public sealed record BookCreated( string Title )
        : IAggregateEvent<BookAggregate, BookId>;
}