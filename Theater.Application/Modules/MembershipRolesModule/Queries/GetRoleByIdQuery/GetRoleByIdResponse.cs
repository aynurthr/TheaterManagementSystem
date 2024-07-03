namespace Theater.Application.Modules.RolesModule.Queries.GetRoleByIdQuery
{
    public class GetRoleByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PolicyDto> Policies { get; set; }
        public IEnumerable<RoleMemberDto> Members { get; set; }
    }
}
