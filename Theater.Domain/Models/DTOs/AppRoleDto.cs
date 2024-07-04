namespace Theater.Domain.Models.DTOs
{
    public class AppRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PolicyDto> Policies { get; set; }
        public IEnumerable<AppRoleMemberDto> Members { get; set; }
    }
}
