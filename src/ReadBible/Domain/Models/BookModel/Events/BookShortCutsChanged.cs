using System.Collections.Generic;
using Akkatecture.Aggregates;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Events
{
    public sealed record BookShortCutsChanged( IReadOnlyCollection<BookShortCut> ShortCuts )
        : IAggregateEvent<BookAggregate, BookId>;
}