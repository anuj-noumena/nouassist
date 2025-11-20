using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Upwork.Dtos
{
    public class UpworkCredentialsDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Upwork.MaxCredentialsLength)]
        public string ApiKey { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.Upwork.MaxCredentialsLength)]
        public string ApiSecret { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public bool IsTokenExpired { get; set; }
    }
}
