using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.AssetManagement.Resources.Services
{
    public class BookMetadataService : DomainService
    {
        public void EnrichBookWithGoogleData(Book book, GoogleBookData googleData)
        {
            if (googleData == null) return;

            book.UpdateGoogleBookData(googleData);
        }

        public string GenerateDisplayTitle(Book book)
        {
            var title = book.Title;

            if (!string.IsNullOrWhiteSpace(book.Subtitle))
            {
                title = $"{title}: {book.Subtitle}";
            }

            if (!string.IsNullOrWhiteSpace(book.Edition))
            {
                title = $"{title} ({book.Edition} Edition)";
            }

            return title;
        }

        public string GenerateCitation(Book book, string style = "apa")
        {
            // Simple APA-style citation
            var authors = book.GetDisplayAuthors();
            var year = book.PublishedDate ?? "n.d.";
            var title = book.Title;
            var publisher = book.GetDisplayPublisher();

            return style.ToLower() switch
            {
                "apa" => $"{authors} ({year}). {title}. {publisher}.",
                "mla" => $"{authors}. {title}. {publisher}, {year}.",
                _ => $"{authors} - {title} ({year})"
            };
        }

        public bool IsValidIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            // Remove hyphens and spaces
            isbn = isbn.Replace("-", "").Replace(" ", "");

            // Check ISBN-10
            if (isbn.Length == 10)
            {
                return IsValidIsbn10(isbn);
            }

            // Check ISBN-13
            if (isbn.Length == 13)
            {
                return IsValidIsbn13(isbn);
            }

            return false;
        }

        private bool IsValidIsbn10(string isbn)
        {
            if (isbn.Length != 10)
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                sum += (isbn[i] - '0') * (10 - i);
            }

            char checkDigit = isbn[9];
            if (checkDigit == 'X')
                sum += 10;
            else if (char.IsDigit(checkDigit))
                sum += checkDigit - '0';
            else
                return false;

            return sum % 11 == 0;
        }

        private bool IsValidIsbn13(string isbn)
        {
            if (isbn.Length != 13)
                return false;

            int sum = 0;
            for (int i = 0; i < 13; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                int digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            return sum % 10 == 0;
        }
    }
}
