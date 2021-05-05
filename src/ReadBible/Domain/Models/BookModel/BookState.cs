using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.ValueObjects;
using ReadBible.Domain.Models.VerseModel;
using ReadBible.Domain.Models.VerseModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookState
        : AggregateState<BookAggregate, BookId>,
          IApply<BookCreated>,
          IApply<BookShortCutsChanged>,
          IApply<VerseAdded>
    {
        private ImmutableDictionary<VerseId, VerseInfo> _verses;

        public BookState()
        {
            ShortCuts = Array.Empty<BookShortCut>();
            _verses   = ImmutableDictionary<VerseId, VerseInfo>.Empty;
        }

        public BookTitle? Title { get; private set; }

        public IReadOnlyCollection<BookShortCut> ShortCuts { get; private set; }

        public bool VerseExists( VerseId verseId ) => _verses.ContainsKey(verseId);

        public void Apply( BookCreated aggregateEvent )
        {
            Title = aggregateEvent.Title;
        }

        public void Apply( BookShortCutsChanged aggregateEvent )
        {
            ShortCuts = aggregateEvent.ShortCuts;
        }

        public void Apply( VerseAdded aggregateEvent )
        {
            _verses = _verses.Add(aggregateEvent.VerseId, new VerseInfo(aggregateEvent.BookChapter, aggregateEvent.VerseNumber));
        }

        private sealed record VerseInfo( BookChapter BookChapter, VerseNumber VerseNumber );
    }
}