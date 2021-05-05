using Akka.Actor;
using Akka.Persistence.TestKit;
using Akkatecture.Aggregates;
using AutoFixture.Xunit2;
using FluentAssertions;
using Monads;
using ReadBible.Domain.Models.BookModel;
using ReadBible.Domain.Models.BookModel.Commands;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.ValueObjects;
using ReadBible.Domain.Models.Common;
using ReadBible.Domain.Models.VerseModel;
using ReadBible.Domain.Models.VerseModel.ValueObjects;
using Xunit;

namespace ReadBible.Tests.Domain.Models.VerseModel
{
    public sealed class BookVersesTest : PersistenceTestKit
    {
        [Theory]
        [AutoData]
        public void When_Book_IsNew_AddVerse_Should_Fail( BookTitle bookTitle, BookChapter bookChapter, VerseNumber verseNumber, string verseText )
        {
            var bookId  = BookId.ForTitle(bookTitle);
            var command = new AddVerse(bookId, bookChapter, verseNumber, verseText);
            var manager = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
            manager.Tell(command);
            ExpectMsg<IResult<Nothing, string>>().IsSuccess().Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void When_Book_IsNotNew_Add_Verse_Should_Emit_VerseAdded_Event( BookTitle bookTitle, BookChapter bookChapter, VerseNumber verseNumber, string verseText )
        {
            var createCommand = new CreateBook(bookTitle);
            var addCommand    = new AddVerse(createCommand.AggregateId, bookChapter, verseNumber, verseText);
            var eventProbe    = CreateTestProbe();
            var manager       = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
            Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent<BookAggregate, BookId, VerseAdded>));
            manager.Tell(createCommand);
            ExpectMsg<IResult<Nothing, string>>();
            manager.Tell(addCommand);
            ExpectMsg<IResult<Nothing, string>>().IsSuccess().Should().BeTrue();
            var e = eventProbe.ExpectMsg<IDomainEvent<BookAggregate, BookId, VerseAdded>>();
            e.AggregateEvent.VerseId.Should().Be(VerseId.Create(createCommand.AggregateId, bookChapter, verseNumber));
            e.AggregateEvent.BookChapter.Should().Be(bookChapter);
            e.AggregateEvent.VerseNumber.Should().Be(verseNumber);
            e.Metadata.Should().Contain("VerseText", verseText);
        }

        [Theory]
        [AutoData]
        public void Add_ExistingVerse_Should_Fail( BookTitle bookTitle, BookChapter bookChapter, VerseNumber verseNumber, string verseText )
        {
            var createCommand = new CreateBook(bookTitle);
            var addCommand    = new AddVerse(createCommand.AggregateId, bookChapter, verseNumber, verseText);
            var addExisting   = new AddVerse(createCommand.AggregateId, bookChapter, verseNumber, "");
            var resultProbe   = CreateTestProbe();
            var manager       = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
            manager.Tell(createCommand);
            manager.Tell(addCommand);
            manager.Tell(addExisting, resultProbe);
            resultProbe.ExpectMsg<IResult<Nothing, string>>().IsSuccess().Should().BeFalse();
        }
    }
}