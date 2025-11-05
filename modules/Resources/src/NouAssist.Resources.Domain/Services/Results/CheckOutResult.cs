using System.Collections.Generic;
using NouAssist.Resources.Aggregates;

namespace NouAssist.Resources.Services.Results
{
    public class CheckOutResult
    {
        public bool IsSuccessful { get; set; }
        public InventoryItem CheckedOutItem { get; set; }
        public List<string> Errors { get; private set; }

        public CheckOutResult()
        {
            Errors = new List<string>();
            IsSuccessful = false;
        }

        public void AddError(string error)
        {
            Errors.Add(error);
            IsSuccessful = false;
        }
    }
}
