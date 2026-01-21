using OnboardingSIGDB1.Domain.Entities.Base;

namespace OnboardingSIGDB1.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}