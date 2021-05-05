using System.Collections.Immutable;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence.TestKit;
using Akkatecture.Aggregates;
using AutoFixture.Xunit2;
using FluentAssertions;
using Monads;
using ReadBible.Domain.Models.BookModel;
using ReadBible.Domain.Models.BookModel.Commands;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.Queries;
using ReadBible.Domain.Models.BookModel.ValueObjects;
using ReadBible.Domain.Models.Common;
using Xunit;

namespace ReadBible.Tests.Domain.Models.BookModel
{
    public sealed class BookModelAggregateTests : PersistenceTestKit
    {
        [Theory]
        [AutoData]
        public async Task When_New_CreateBook_Should_Emit_BookCreated_Event( string bookTitle )
        {
            await WithJournalWrite(w => w.Pass(), () =>
            {
                //arrange
                var resultProbe = CreateTestProbe("result-probe");
                var eventProbe  = CreateTestProbe("event-probe");
                var title       = new BookTitle(bookTitle);
                var command     = new CreateBook(title);
                var bookManager = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
                Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent));
                //act
                bookManager.Tell(command, resultProbe);

                //assert
                resultProbe.ExpectMsg<IResult<Nothing, string>>()
                           .IsSuccess().Should().BeTrue();

                eventProbe.ExpectMsg<IDomainEvent<BookAggregate, BookId, BookCreated>>()
                          .AggregateEvent.Title.Should().Be(title);
            });
        }

        [Theory]
        [AutoData]
        public async Task When_New_CreateBook_Should_Emit_BookShortCutsChanged_Event( BookTitle bookTitle, BookShortCut[] shortCuts )
        {
            await WithJournalWrite(w => w.Pass(), () =>
            {
                //arrange
                var resultProbe   = CreateTestProbe("result-probe");
                var eventProbe    = CreateTestProbe("event-probe");
                var bookShortCuts = shortCuts.ToImmutableHashSet();
                var command       = new CreateBook(bookTitle) {ShortCuts = bookShortCuts};
                var bookManager   = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
                Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent<BookAggregate, BookId, BookShortCutsChanged>));
                //act
                bookManager.Tell(command, resultProbe);

                //assert
                resultProbe.ExpectMsg<IResult<Nothing, string>>()
                           .IsSuccess().Should().BeTrue();

                eventProbe.ExpectMsg<IDomainEvent<BookAggregate, BookId, BookShortCutsChanged>>()
                          .AggregateEvent.ShortCuts.Should().BeSubsetOf(bookShortCuts);
            });
        }

        [Theory]
        [AutoData]
        public async Task When_NotNew_CreateBook_Should_Fail( string bookTitle )
        {
            await WithJournalWrite(w => w.Pass(), () =>
            {
                //arrange
                var resultProbe = CreateTestProbe("result-probe");
                var eventProbe  = CreateTestProbe("event-probe");
                var title       = new BookTitle(bookTitle);
                var command     = new CreateBook(title);
                var bookManager = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
                Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent));
                //act
                bookManager.Tell(command);
                eventProbe.ExpectMsg<IDomainEvent<BookAggregate, BookId, BookCreated>>();
                bookManager.Tell(command, resultProbe);
                //assert
                resultProbe.ExpectMsg<IResult<Nothing, string>>()
                           .IsSuccess().Should().BeFalse();
                eventProbe.ExpectNoMsg();
            });
        }

        [Theory]
        [AutoData]
        public void When_New_GetBookByTitle_Should_Fail( BookTitle bookTitle )
        {
            //arrange
            var query       = new GetBookByTitle(bookTitle);
            var bookManager = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
            //act
            bookManager.Tell(query);
            //assert
            ExpectMsg<IResult<Book, string>>().IsSuccess().Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void When_NotNew_GetBookTitle_Should_Success( BookTitle bookTitle, BookShortCut[] shortCuts )
        {
            //arrange
            var bookShortCuts = shortCuts.ToImmutableHashSet();
            var createCommand = new CreateBook(bookTitle) {ShortCuts = bookShortCuts};
            var query         = new GetBookByTitle(bookTitle);
            var resultProbe   = CreateTestProbe("result-probe");
            var bookManager   = ActorOf(Props.Create<BookAggregateManager>(), "book-manager");
            //act
            bookManager.Tell(createCommand);
            bookManager.Tell(query, resultProbe);
            var book = resultProbe.ExpectMsg<IResult<Book, string>>().Fold(b => b, _ => new Book(new BookTitle("Title")));
            //assert
            book.Should().NotBeNull();
            book.Title.Should().Be(bookTitle);
            book.ShortCuts.Should().BeSubsetOf(bookShortCuts);
        }
    }
}