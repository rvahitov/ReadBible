using Akkatecture.Aggregates;
using ReadBible.Domain.Models.VerseModel;
using ReadBible.Domain.Models.VerseModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Events
{
    public sealed record VerseAdded( VerseId VerseId, BookChapter BookChapter, VerseNumber VerseNumber)
        : IAggregateEvent<BookAggregate, BookId>;
}