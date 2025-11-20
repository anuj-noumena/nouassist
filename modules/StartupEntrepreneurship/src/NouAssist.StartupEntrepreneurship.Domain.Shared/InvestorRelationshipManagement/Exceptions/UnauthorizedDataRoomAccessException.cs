namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when unauthorized access to data room is attempted
    /// </summary>
    public class UnauthorizedDataRoomAccessException : IRMDomainException
    {
        public UnauthorizedDataRoomAccessException(Guid investorId, Guid dataRoomId)
            : base("IRM:ErrorMessage:UnauthorizedDataRoomAccess")
        {
            WithData("investorId", investorId);
            WithData("dataRoomId", dataRoomId);
        }
    }
}
