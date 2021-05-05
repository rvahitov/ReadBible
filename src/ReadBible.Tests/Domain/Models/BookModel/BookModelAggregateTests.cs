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
using ReadBible.Domain.Models.BookModel.ValueObjects;
using ReadBible.Domain.Models.Common;
using Xunit;

namespace ReadBible.Tests.Domain.Models.BookModel
{
    public sealed class BookModelAggregateTests : PersistenceTestKit
    {
        [Theory]
        [AutoData]
        public async Task When_New_CreateBook_Should_Emit_BookCreated_Event(string bookTitle)
        {
            await WithJournalWrite(w => w.Pass(), () =>
            {
                //arrange
                var resultProbe = CreateTestProbe("result-probe");
                var eventProbe  = CreateTestProbe("event-probe");
                var title       = new BookTitle(bookTitle);
                var command     = new CreateBook(title);
                var bookManager = ActorOf(Props.Create<BookAggregateManager>(),"book-manager");
                Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent<BookAggregate, BookId, BookCreated>));
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
        public async Task When_NotNew_CreateBook_Should_Fail(string bookTitle)
        {
            await WithJournalWrite(w => w.Pass(), () =>
            {
                //arrange
                var resultProbe = CreateTestProbe("result-probe");
                var eventProbe  = CreateTestProbe("event-probe");
                var title       = new BookTitle(bookTitle);
                var command     = new CreateBook(title);
                var bookManager = ActorOf(Props.Create<BookAggregateManager>(),"book-manager");
                Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent<BookAggregate, BookId, BookCreated>));
                //act
                bookManager.Tell(command);
                eventProbe.ExpectMsg<IDomainEvent<BookAggregate, BookId, BookCreated>>();
                bookManager.Tell(command,resultProbe);
                //assert
                resultProbe.ExpectMsg<IResult<Nothing, string>>()
                           .IsSuccess().Should().BeFalse();
                eventProbe.ExpectNoMsg();
            });
        }
    }
}