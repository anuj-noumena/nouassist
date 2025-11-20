using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AccountsPayable.Services
{
    public class BillApprovalService : DomainService
    {
        public bool RequiresApproval(Bill bill, decimal approvalThreshold)
        {
            return bill.TotalAmount.Amount >= approvalThreshold;
        }

        public ApprovalLevel DetermineRequiredApprovalLevel(Bill bill)
        {
            var amount = bill.TotalAmount.Amount;

            return amount switch
            {
                < 1000 => ApprovalLevel.Level1,
                < 5000 => ApprovalLevel.Level2,
                < 25000 => ApprovalLevel.Level3,
                _ => ApprovalLevel.Level4
            };
        }

        public bool CanUserApprove(Guid userId, ApprovalLevel billApprovalLevel, ApprovalLevel userApprovalLevel)
        {
            return userApprovalLevel >= billApprovalLevel;
        }

        public void ProcessApproval(Bill bill, Guid approverId, ApprovalLevel approverLevel, string? comments = null)
        {
            var requiredLevel = DetermineRequiredApprovalLevel(bill);

            if (!CanUserApprove(approverId, requiredLevel, approverLevel))
            {
                throw new BusinessException("AccountsPayable:InsufficientApprovalAuthority",
                    $"User does not have sufficient approval authority. Required: {requiredLevel}, User has: {approverLevel}");
            }

            bill.Approve(approverId, approverLevel, comments);
        }

        public List<Bill> FilterBillsRequiringApproval(List<Bill> bills, decimal approvalThreshold)
        {
            return bills.Where(b => RequiresApproval(b, approvalThreshold)).ToList();
        }

        public Dictionary<ApprovalLevel, List<Bill>> GroupBillsByApprovalLevel(List<Bill> bills)
        {
            return bills
                .GroupBy(b => DetermineRequiredApprovalLevel(b))
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}