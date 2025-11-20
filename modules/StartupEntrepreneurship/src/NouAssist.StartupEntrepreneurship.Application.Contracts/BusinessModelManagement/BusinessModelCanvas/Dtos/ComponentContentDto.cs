using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class ComponentContentDto
    {
        public string Text { get; set; }
        public List<string> BulletPoints { get; set; }
        public Dictionary<string, string> AdditionalProperties { get; set; }

        public ComponentContentDto()
        {
            BulletPoints = new List<string>();
            AdditionalProperties = new Dictionary<string, string>();
        }
    }
}
