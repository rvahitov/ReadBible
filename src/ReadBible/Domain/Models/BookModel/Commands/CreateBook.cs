using System.Collections.Immutable;
using Akkatecture.Commands;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Commands
{
    public sealed class CreateBook : Command<BookAggregate, BookId>
    {
        public CreateBook( BookTitle title ) : base(BookId.ForTitle(title))
        {
            Title = title;
        }

        public BookTitle Title { get; }

        public ImmutableHashSet<BookShortCut> ShortCuts { get; init; } =
            ImmutableHashSet<BookShortCut>.Empty;
    }
}