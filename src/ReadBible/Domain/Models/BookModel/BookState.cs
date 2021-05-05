using System;
using System.Collections.Generic;
using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookState
        : AggregateState<BookAggregate, BookId>,
          IApply<BookCreated>,
          IApply<BookShortCutsChanged>
    {
        public BookState()
        {
            ShortCuts = Array.Empty<BookShortCut>();
        }

        public BookTitle? Title { get; private set; }
        
        public IReadOnlyCollection<BookShortCut> ShortCuts { get; private set; }

        public void Apply( BookCreated aggregateEvent )
        {
            Title = aggregateEvent.Title;
        }

        public void Apply( BookShortCutsChanged aggregateEvent )
        {
            ShortCuts = aggregateEvent.ShortCuts;
        }
    }
}