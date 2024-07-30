namespace Theater.Domain.Models.DTOs
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Phone { get; set; }
        public bool PhoneConfirmed { get; set; }
        public IEnumerable<PolicyDto> Policies { get; set; }
        public IEnumerable<AppUserRoleDto> Roles { get; set; }
    }
}
