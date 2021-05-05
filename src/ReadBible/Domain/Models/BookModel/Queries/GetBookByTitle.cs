using Akkatecture.Commands;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Queries
{
    public sealed class GetBookByTitle : Command<BookAggregate, BookId>
    {
        public GetBookByTitle( BookTitle title ) : base(BookId.ForTitle(title))
        {
            Title = title;
        }

        public BookTitle Title { get; }
    }
}