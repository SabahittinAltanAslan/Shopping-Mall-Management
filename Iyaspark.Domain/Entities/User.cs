using Iyaspark.Domain.Enums;
using Iyaspark.SharedKernel;
using Iyaspark.SharedKernel.Entities;

namespace Iyaspark.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
