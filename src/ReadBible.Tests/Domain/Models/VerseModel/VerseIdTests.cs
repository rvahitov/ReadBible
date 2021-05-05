using AutoFixture.Xunit2;
using FluentAssertions;
using ReadBible.Domain.Models.BookModel;
using ReadBible.Domain.Models.BookModel.ValueObjects;
using ReadBible.Domain.Models.VerseModel;
using ReadBible.Domain.Models.VerseModel.ValueObjects;
using Xunit;

namespace ReadBible.Tests.Domain.Models.VerseModel
{
    public sealed class VerseIdTests
    {
        [Theory]
        [AutoData]
        public void VerseId_Should_Be_Equal( BookTitle bookTitle, BookChapter bookChapter, VerseNumber verseNumber )
        {
            VerseId v1 = VerseId.Create(BookId.ForTitle(bookTitle), bookChapter, verseNumber);
            VerseId v2 = new(v1.Value);
            v2.Should().Be(v1);
        }
    }
}