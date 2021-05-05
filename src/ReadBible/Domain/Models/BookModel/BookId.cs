using System;
using Akkatecture.Core;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class BookId : Identity<BookId>
    {
        private static readonly Guid TitleNamespace = new("D37BE747-66DD-41DD-80B2-C98B54F72C71");

        public BookId( string value ) : base(value)
        {
        }

        public static BookId ForTitle( BookTitle title ) => NewDeterministic(TitleNamespace, title.Value);
    }
}