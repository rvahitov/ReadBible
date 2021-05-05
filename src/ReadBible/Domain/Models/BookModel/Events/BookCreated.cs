using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Events
{
    public sealed record BookCreated( BookTitle Title )
        : IAggregateEvent<BookAggregate, BookId>;
}