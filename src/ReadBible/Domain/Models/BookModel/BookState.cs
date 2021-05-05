using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.Events;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookState
        : AggregateState<BookAggregate, BookId>,
          IApply<BookCreated>
    {
        public string? Title { get; private set; }

        public void Apply( BookCreated aggregateEvent )
        {
            Title = aggregateEvent.Title;
        }
    }
}