using Akkatecture.Commands;
using ReadBible.Domain.Models.VerseModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel.Commands
{
    public sealed class AddVerse : Command<BookAggregate, BookId>
    {
        public AddVerse( BookId aggregateId, BookChapter bookChapter, VerseNumber verseNumber, string verseText ) : base(aggregateId)
        {
            BookChapter = bookChapter;
            VerseNumber = verseNumber;
            VerseText   = verseText;
        }

        public BookChapter BookChapter { get; }
        public VerseNumber VerseNumber { get; }
        public string      VerseText   { get; }
    }
}