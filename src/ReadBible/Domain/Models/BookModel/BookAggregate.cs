using Akkatecture.Aggregates;
using Monads;
using ReadBible.Domain.Models.BookModel.Commands;
using ReadBible.Domain.Models.BookModel.Events;
using ReadBible.Domain.Models.BookModel.Queries;
using ReadBible.Domain.Models.Common;
using ReadBible.Domain.Models.VerseModel;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookAggregate : AggregateRoot<BookAggregate, BookId, BookState>
    {
        public BookAggregate( BookId id ) : base(id)
        {
            Command<CreateBook>(OnCreateBook);
            Command<GetBookByTitle>(OnGetByTitle);
            Command<AddVerse>(OnAddVerse);
        }

        private void OnCreateBook( CreateBook command )
        {
            var x = from _ in IsNew.Validate(isNew => isNew, $"Book {Id} already exists")
                    let created = new BookCreated(command.Title)
                    select created;
            x.Fold(e =>
                {
                    Reply(ExecutionResult.Success());
                    if ( command.ShortCuts.IsEmpty )
                    {
                        Emit(e);
                    }
                    else
                    {
                        EmitAll(e, new BookShortCutsChanged(command.ShortCuts));
                    }
                },
                error => { Sender.Tell(ExecutionResult.Failure(error), Self); });
        }

        private void OnGetByTitle( GetBookByTitle query )
        {
            if ( IsNew )
            {
                Sender.Tell(new FailureResult<Book, string>($"Book {query.Title} not found"), Self);
            }
            else
            {
                var book = new Book(State.Title!) {ShortCuts = State.ShortCuts};
                Sender.Tell(new SuccessResult<Book, string>(book), Self);
            }
        }

        private void OnAddVerse( AddVerse command )
        {
            if ( IsNew )
            {
                Sender.Tell(ExecutionResult.Failure($"Book {command.AggregateId} is not exists"), Self);
                return;
            }
            
            var verseId = VerseId.Create(Id, command.BookChapter, command.VerseNumber);
            if ( State.VerseExists(verseId) )
            {
                Sender.Tell(ExecutionResult.Failure($"Book {Id} already have verse {verseId}"), Self);
                return;
            }
            
            Reply(ExecutionResult.Success());
            var e       = new VerseAdded(verseId, command.BookChapter, command.VerseNumber);
            var meta = new Metadata
            {
                {"VerseText", command.VerseText}
            };
            Emit(e, meta);
        }
    }
}