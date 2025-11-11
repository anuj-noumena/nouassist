using System;
using System.Linq.Expressions;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.Resources.Specifications
{
    public class BooksByReadingStatusSpecification : Specification<Book>
    {
        private readonly ReadingStatus _status;

        public BooksByReadingStatusSpecification(ReadingStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.ReadingStatus == _status && !b.IsDeleted;
    }

    public class CurrentlyReadingBooksSpecification : Specification<Book>
    {
        private readonly Guid _ownerId;

        public CurrentlyReadingBooksSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.OwnerId == _ownerId && 
                 b.ReadingStatus == ReadingStatus.Reading && 
                 !b.IsDeleted;
    }

    public class FavoriteBooksSpecification : Specification<Book>
    {
        private readonly Guid _ownerId;

        public FavoriteBooksSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.OwnerId == _ownerId && 
                 b.IsFavorite == true && 
                 !b.IsDeleted;
    }

    public class BooksByAuthorSpecification : Specification<Book>
    {
        private readonly string _authorName;

        public BooksByAuthorSpecification(string authorName)
        {
            _authorName = authorName;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.Authors.Contains(_authorName) && !b.IsDeleted;
    }

    public class BooksInSeriesSpecification : Specification<Book>
    {
        private readonly string _seriesName;

        public BooksInSeriesSpecification(string seriesName)
        {
            _seriesName = seriesName;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.SeriesName == _seriesName && !b.IsDeleted;
    }

    public class HighlyRatedBooksSpecification : Specification<Book>
    {
        private readonly int _minRating;

        public HighlyRatedBooksSpecification(int minRating = 4)
        {
            _minRating = minRating;
        }

        public override Expression<Func<Book, bool>> ToExpression() =>
            b => b.UserRating.HasValue && 
                 b.UserRating.Value >= _minRating && 
                 !b.IsDeleted;
    }
}
