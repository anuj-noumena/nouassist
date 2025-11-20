using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Repositories
{
    public class EfCoreSpecificationStructureRepository 
        : EfCoreRepository<IRequirementSpecificationManagementDbContext, SpecificationStructure, Guid>, ISpecificationStructureRepository
    {
        public EfCoreSpecificationStructureRepository(
            IDbContextProvider<IRequirementSpecificationManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SpecificationStructure> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.Name == name)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<SpecificationStructure>> GetByDocumentTypeAsync(
            string documentType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.DocumentType == documentType)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Override to always include sections by default
        /// </summary>
        public override async Task<IQueryable<SpecificationStructure>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(s => s.Sections);
        }

        /// <summary>
        /// Get structures with their sections in hierarchical order
        /// </summary>
        public async Task<List<SpecificationStructure>> GetWithSectionsAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            var structures = await query.ToListAsync(cancellationToken);

            // Load sections in hierarchical order
            foreach (var structure in structures)
            {
                await LoadSectionsHierarchicallyAsync(structure, cancellationToken);
            }

            return structures;
        }

        /// <summary>
        /// Get a structure by name with all sections loaded
        /// </summary>
        public async Task<SpecificationStructure> GetByNameWithSectionsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            var structure = await query
                .Where(s => s.Name == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (structure != null)
            {
                await LoadSectionsHierarchicallyAsync(structure, cancellationToken);
            }

            return structure;
        }

        /// <summary>
        /// Get structures created by a specific user
        /// </summary>
        public async Task<List<SpecificationStructure>> GetByCreatorAsync(
            Guid creatorId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.CreatorId == creatorId)
                .OrderByDescending(s => s.CreationTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Search structures by name or description
        /// </summary>
        public async Task<List<SpecificationStructure>> SearchAsync(
            string searchText,
            string documentType = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (!searchText.IsNullOrWhiteSpace())
            {
                query = query.Where(s => 
                    s.Name.Contains(searchText) ||
                    s.Description.Contains(searchText));
            }

            if (!documentType.IsNullOrWhiteSpace())
            {
                query = query.Where(s => s.DocumentType == documentType);
            }

            return await query
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Helper method to load sections in hierarchical order
        /// </summary>
        private async Task LoadSectionsHierarchicallyAsync(
            SpecificationStructure structure,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // This is a workaround since we can't modify the private _sections collection
            // In practice, you might need to expose a method in the aggregate to reload sections
            
            await dbContext.Entry(structure)
                .Collection("_sections")
                .Query()
                .OrderBy("ParentSectionId")
                .ThenBy("Order")
                .LoadAsync(cancellationToken);
        }
    }
}
