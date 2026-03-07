using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Authorization
{
    public class UserOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
