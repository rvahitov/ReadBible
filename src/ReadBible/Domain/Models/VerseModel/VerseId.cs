using System;
using Akkatecture.Core;
using ReadBible.Domain.Models.BookModel;
using ReadBible.Domain.Models.VerseModel.ValueObjects;

namespace ReadBible.Domain.Models.VerseModel
{
    public sealed class VerseId : Identity<VerseId>
    {
        private static readonly Guid DefaultNamespace = new("8D4C6AA4-63F1-4B84-ABDE-45BF8AA59C8C");
        public VerseId( string value ) : base(value)
        {
        }

        public static VerseId Create( BookId bookTitle, BookChapter bookChapter, VerseNumber verseNumber )
        {
            var s = $"{bookTitle}-{bookChapter.Value}-{verseNumber.Value}";
            return NewDeterministic(DefaultNamespace, s);
        }
    }
}