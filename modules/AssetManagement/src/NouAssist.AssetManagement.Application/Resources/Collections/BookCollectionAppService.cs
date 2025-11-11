using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.AssetManagement.Permissions;
using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Collections.Dtos;
using NouAssist.AssetManagement.Resources.Data;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement.Resources.Collections
{
    [Authorize(AssetManagementPermissions.Resources.Collections.Default)]
    public class BookCollectionAppService : ApplicationService, IBookCollectionAppService
    {
        private readonly IBookCollectionRepository _collectionRepository;
        private readonly IBookRepository _bookRepository;

        public BookCollectionAppService(
            IBookCollectionRepository collectionRepository,
            IBookRepository bookRepository)
        {
            _collectionRepository = collectionRepository;
            _bookRepository = bookRepository;
        }

        public async Task<BookCollectionDto> GetAsync(Guid id)
        {
            var collection = await _collectionRepository.GetAsync(id);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value && !collection.IsPublic)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to access this collection.");
            }

            return ObjectMapper.Map<BookCollection, BookCollectionDto>(collection);
        }

        public async Task<List<BookCollectionDto>> GetMyCollectionsAsync()
        {
            var collections = await _collectionRepository.GetByOwnerAsync(CurrentUser.Id.Value);
            return ObjectMapper.Map<List<BookCollection>, List<BookCollectionDto>>(collections);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Create)]
        public async Task<BookCollectionDto> CreateAsync(CreateBookCollectionDto input)
        {
            // Check for duplicate name
            var existing = await _collectionRepository.FindByNameAsync(CurrentUser.Id.Value, input.Name);
            if (existing != null)
            {
                throw new BusinessException("Collections:DuplicateName", "A collection with this name already exists.");
            }

            var collection = new BookCollection(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                input.Name,
                CurrentTenant.Id,
                input.Description
            );

            if (input.Tags != null && input.Tags.Count > 0)
            {
                collection.SetTags(input.Tags);
            }

            if (input.IsPublic)
            {
                collection.MakePublic();
            }

            await _collectionRepository.InsertAsync(collection);

            return ObjectMapper.Map<BookCollection, BookCollectionDto>(collection);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Update)]
        public async Task<BookCollectionDto> UpdateAsync(Guid id, UpdateBookCollectionDto input)
        {
            var collection = await _collectionRepository.GetAsync(id);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to update this collection.");
            }

            // Check for duplicate name
            var existing = await _collectionRepository.FindByNameAsync(CurrentUser.Id.Value, input.Name);
            if (existing != null && existing.Id != id)
            {
                throw new BusinessException("Collections:DuplicateName", "A collection with this name already exists.");
            }

            collection.UpdateName(input.Name);
            collection.UpdateDescription(input.Description);

            if (input.Tags != null)
            {
                collection.SetTags(input.Tags);
            }

            if (input.IsPublic)
            {
                collection.MakePublic();
            }
            else
            {
                collection.MakePrivate();
            }

            await _collectionRepository.UpdateAsync(collection);

            return ObjectMapper.Map<BookCollection, BookCollectionDto>(collection);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var collection = await _collectionRepository.GetAsync(id);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to delete this collection.");
            }

            collection.Delete(CurrentUser.Id.Value.ToString());
            await _collectionRepository.UpdateAsync(collection);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Update)]
        public async Task<BookCollectionDto> AddBookAsync(Guid collectionId, AddBookToCollectionDto input)
        {
            var collection = await _collectionRepository.GetAsync(collectionId);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to modify this collection.");
            }

            // Verify book exists and belongs to user
            var book = await _bookRepository.GetAsync(input.BookId);
            if (book.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:BookNotOwned", "You can only add your own books to collections.");
            }

            collection.AddBook(input.BookId);
            await _collectionRepository.UpdateAsync(collection);

            return ObjectMapper.Map<BookCollection, BookCollectionDto>(collection);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Update)]
        public async Task RemoveBookAsync(Guid collectionId, Guid bookId)
        {
            var collection = await _collectionRepository.GetAsync(collectionId);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to modify this collection.");
            }

            collection.RemoveBook(bookId);
            await _collectionRepository.UpdateAsync(collection);
        }

        [Authorize(AssetManagementPermissions.Resources.Collections.Update)]
        public async Task<BookCollectionDto> ReorderAsync(Guid id, int newSortOrder)
        {
            var collection = await _collectionRepository.GetAsync(id);

            // Authorization check
            if (collection.OwnerId != CurrentUser.Id.Value)
            {
                throw new BusinessException("Collections:NotAuthorized", "You are not authorized to reorder this collection.");
            }

            collection.SetSortOrder(newSortOrder);
            await _collectionRepository.UpdateAsync(collection);

            return ObjectMapper.Map<BookCollection, BookCollectionDto>(collection);
        }
    }
}
