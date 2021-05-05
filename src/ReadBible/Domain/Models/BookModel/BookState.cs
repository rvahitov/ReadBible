using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookState
        : AggregateState<BookAggregate, BookId>,
          IApply<BookCreated>
    {
        public BookTitle? Title { get; private set; }

        public void Apply( BookCreated aggregateEvent )
        {
            Title = aggregateEvent.Title;
        }
    }
}