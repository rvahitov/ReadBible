using System;
using System.Collections.Generic;
using Akkatecture.Entities;
using ReadBible.Domain.Models.BookModel.ValueObjects;

namespace ReadBible.Domain.Models.BookModel
{
    public sealed class Book : Entity<BookId>
    {
        public Book( BookTitle title ) : base(BookId.ForTitle(title))
        {
            Title = title;
        }

        public BookTitle Title { get; }

        public IReadOnlyCollection<BookShortCut> ShortCuts { get; init; } = Array.Empty<BookShortCut>();
    }
}