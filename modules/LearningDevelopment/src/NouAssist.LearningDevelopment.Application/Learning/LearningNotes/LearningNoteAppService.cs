using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using NouAssist.Learning.LearningNotes.Dtos;
using NouAssist.Learning.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.LearningNotes
{
    [Authorize(LearningPermissions.LearningNotes.Default)]
    public class LearningNoteAppService : ApplicationService, ILearningNoteAppService
    {
        private readonly ILearningNoteRepository _noteRepository;

        public LearningNoteAppService(ILearningNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<LearningNoteDto> GetAsync(Guid id)
        {
            var note = await _noteRepository.GetAsync(id);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        public async Task<PagedResultDto<LearningNoteDto>> GetListAsync(GetLearningNoteListDto input)
        {
            var query = (await _noteRepository.GetQueryableAsync())
                .Where(n => n.UserId == CurrentUser.GetId());

            if (input.TopicId.HasValue)
            {
                query = query.Where(n => n.TopicId == input.TopicId.Value);
            }

            if (input.Type.HasValue)
            {
                query = query.Where(n => n.Type == input.Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
            {
                query = query.Where(n => 
                    n.Notes.Contains(input.SearchKeyword) ||
                    n.Content.Contains(input.SearchKeyword) ||
                    n.Summary.Contains(input.SearchKeyword));
            }

            var totalCount = query.Count();

            var notes = query
                .OrderByDescending(n => n.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<LearningNoteDto>(
                totalCount,
                ObjectMapper.Map<List<LearningNote>, List<LearningNoteDto>>(notes)
            );
        }

        [Authorize(LearningPermissions.LearningNotes.Create)]
        public async Task<LearningNoteDto> CreateAsync(CreateLearningNoteDto input)
        {
            var note = new LearningNote(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.TopicId,
                input.Type,
                CurrentTenant.Id
            );

            if (input.Type == Enums.LearningNoteType.Cornell)
            {
                note.UpdateCornellNote(input.Questions, input.Notes, input.Summary);
            }
            else
            {
                note.UpdateContent(input.Content);
            }

            foreach (var keyPoint in input.KeyPoints ?? new List<string>())
            {
                note.AddKeyPoint(keyPoint);
            }

            foreach (var kvp in input.Metadata ?? new Dictionary<string, object>())
            {
                note.SetMetadata(kvp.Key, kvp.Value);
            }

            await _noteRepository.InsertAsync(note);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        [Authorize(LearningPermissions.LearningNotes.Update)]
        public async Task<LearningNoteDto> UpdateAsync(Guid id, UpdateLearningNoteDto input)
        {
            var note = await _noteRepository.GetAsync(id);

            if (note.Type == Enums.LearningNoteType.Cornell)
            {
                note.UpdateCornellNote(input.Questions, input.Notes, input.Summary);
            }
            else
            {
                note.UpdateContent(input.Content);
            }

            // Update key points (simplified - in production, handle add/remove)
            foreach (var keyPoint in input.KeyPoints ?? new List<string>())
            {
                note.AddKeyPoint(keyPoint);
            }

            // Update metadata
            foreach (var kvp in input.Metadata ?? new Dictionary<string, object>())
            {
                note.SetMetadata(kvp.Key, kvp.Value);
            }

            await _noteRepository.UpdateAsync(note);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        [Authorize(LearningPermissions.LearningNotes.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _noteRepository.DeleteAsync(id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using NouAssist.Learning.LearningNotes.Dtos;
using NouAssist.Learning.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Learning.LearningNotes
{
    [Authorize(LearningPermissions.LearningNotes.Default)]
    public class LearningNoteAppService : ApplicationService, ILearningNoteAppService
    {
        private readonly ILearningNoteRepository _noteRepository;

        public LearningNoteAppService(ILearningNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<LearningNoteDto> GetAsync(Guid id)
        {
            var note = await _noteRepository.GetAsync(id);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        public async Task<PagedResultDto<LearningNoteDto>> GetListAsync(GetLearningNoteListDto input)
        {
            var query = (await _noteRepository.GetQueryableAsync())
                .Where(n => n.UserId == CurrentUser.GetId());

            if (input.TopicId.HasValue)
            {
                query = query.Where(n => n.TopicId == input.TopicId.Value);
            }

            if (input.Type.HasValue)
            {
                query = query.Where(n => n.Type == input.Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.SearchKeyword))
            {
                query = query.Where(n => 
                    n.Notes.Contains(input.SearchKeyword) ||
                    n.Content.Contains(input.SearchKeyword) ||
                    n.Summary.Contains(input.SearchKeyword));
            }

            // Use async methods
            var totalCount = await query.CountAsync();

            var notes = await query
                .OrderByDescending(n => n.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            return new PagedResultDto<LearningNoteDto>(
                totalCount,
                ObjectMapper.Map<List<LearningNote>, List<LearningNoteDto>>(notes)
            );
        }

        [Authorize(LearningPermissions.LearningNotes.Create)]
        public async Task<LearningNoteDto> CreateAsync(CreateLearningNoteDto input)
        {
            var note = new LearningNote(
                GuidGenerator.Create(),
                CurrentUser.GetId(),
                input.TopicId,
                input.Type,
                CurrentTenant.Id
            );

            if (input.Type == Enums.LearningNoteType.Cornell)
            {
                note.UpdateCornellNote(input.Questions, input.Notes, input.Summary);
            }
            else
            {
                note.UpdateContent(input.Content);
            }

            foreach (var keyPoint in input.KeyPoints ?? new List<string>())
            {
                note.AddKeyPoint(keyPoint);
            }

            foreach (var kvp in input.Metadata ?? new Dictionary<string, object>())
            {
                note.SetMetadata(kvp.Key, kvp.Value);
            }

            await _noteRepository.InsertAsync(note);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        [Authorize(LearningPermissions.LearningNotes.Update)]
        public async Task<LearningNoteDto> UpdateAsync(Guid id, UpdateLearningNoteDto input)
        {
            var note = await _noteRepository.GetAsync(id);

            if (note.Type == Enums.LearningNoteType.Cornell)
            {
                note.UpdateCornellNote(input.Questions, input.Notes, input.Summary);
            }
            else
            {
                note.UpdateContent(input.Content);
            }

            // Update key points
            foreach (var keyPoint in input.KeyPoints ?? new List<string>())
            {
                note.AddKeyPoint(keyPoint);
            }

            // Update metadata
            foreach (var kvp in input.Metadata ?? new Dictionary<string, object>())
            {
                note.SetMetadata(kvp.Key, kvp.Value);
            }

            await _noteRepository.UpdateAsync(note);
            return ObjectMapper.Map<LearningNote, LearningNoteDto>(note);
        }

        [Authorize(LearningPermissions.LearningNotes.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _noteRepository.DeleteAsync(id);
        }
    }
}
