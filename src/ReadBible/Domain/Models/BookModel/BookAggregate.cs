using Akkatecture.Aggregates;
using Monads;
using ReadBible.Domain.Models.BookModel.Commands;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.Common;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookAggregate : AggregateRoot<BookAggregate, BookId, BookState>
    {
        public BookAggregate( BookId id ) : base(id)
        {
            Command<CreateBook>(OnCreateBook);
        }

        private void OnCreateBook( CreateBook command )
        {
            var x = from _ in IsNew.Validate(isNew => isNew, $"Book {Id} already exists")
                    let e = new BookCreated(command.Title)
                    select e;
            x.Fold(e =>
                {
                    Reply(ExecutionResult.Success());
                    Emit(e);
                },
                error => { Sender.Tell(ExecutionResult.Failure(error), Self); });
        }
    }
}