using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Enums;

namespace NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos
{
    /// <summary>
    /// DTO for displaying a traceability matrix
    /// </summary>
    public class TraceabilityMatrixDto
    {
        /// <summary>
        /// Type of source artifacts
        /// </summary>
        public ArtifactType SourceType { get; set; }
        
        /// <summary>
        /// Type of target artifacts
        /// </summary>
        public ArtifactType TargetType { get; set; }
        
        /// <summary>
        /// Type of links included in the matrix, if filtered
        /// </summary>
        public LinkType? LinkType { get; set; }
        
        /// <summary>
        /// List of source artifact IDs with their names
        /// </summary>
        public List<ArtifactReferenceDto> SourceArtifacts { get; set; } = new List<ArtifactReferenceDto>();
        
        /// <summary>
        /// List of target artifact IDs with their names
        /// </summary>
        public List<ArtifactReferenceDto> TargetArtifacts { get; set; } = new List<ArtifactReferenceDto>();
        
        /// <summary>
        /// Matrix cells with link information
        /// </summary>
        public List<MatrixCellDto> Cells { get; set; } = new List<MatrixCellDto>();
    }
    
    /// <summary>
    /// DTO for an artifact reference in a matrix
    /// </summary>
    public class ArtifactReferenceDto
    {
        /// <summary>
        /// Artifact ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Artifact name
        /// </summary>
        public string Name { get; set; }
    }
    
    /// <summary>
    /// DTO for a cell in a traceability matrix
    /// </summary>
    public class MatrixCellDto
    {
        /// <summary>
        /// Source artifact ID
        /// </summary>
        public string SourceId { get; set; }
        
        /// <summary>
        /// Target artifact ID
        /// </summary>
        public string TargetId { get; set; }
        
        /// <summary>
        /// Link types and IDs for this cell
        /// </summary>
        public List<MatrixCellLinkDto> Links { get; set; } = new List<MatrixCellLinkDto>();
    }
    
    /// <summary>
    /// DTO for a link in a matrix cell
    /// </summary>
    public class MatrixCellLinkDto
    {
        /// <summary>
        /// Type of link
        /// </summary>
        public LinkType Type { get; set; }
        
        /// <summary>
        /// Link ID
        /// </summary>
        public Guid LinkId { get; set; }
    }
}
