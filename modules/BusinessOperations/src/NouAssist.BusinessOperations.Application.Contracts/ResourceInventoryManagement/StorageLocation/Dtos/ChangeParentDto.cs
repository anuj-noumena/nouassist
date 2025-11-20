using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for changing the parent location
    /// </summary>
    public class ChangeParentDto
    {
        /// <summary>
        /// New parent location ID (null for root location)
        /// </summary>
        public Guid? ParentLocationId { get; set; }
    }
}
