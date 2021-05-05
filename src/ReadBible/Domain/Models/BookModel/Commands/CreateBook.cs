using Akkatecture.Commands;

namespace ReadBible.Domain.Models.BookModel.Commands
{
    public sealed class CreateBook : Command<BookAggregate, BookId>
    {
        public CreateBook( string title ) : base(BookId.ForTitle(title))
        {
            Title = title;
        }

        public string Title { get; }
    }
}