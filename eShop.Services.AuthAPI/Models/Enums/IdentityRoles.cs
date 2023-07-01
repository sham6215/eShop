using eShop.Services.AuthAPI.Extensions;

namespace eShop.Services.AuthAPI.Models.Enums
{
    public enum IdentityRoles
    {
        [StringValue("Admin")]
        Admin = 1,
        [StringValue("Customer")]
        Customer = 2
    }
}
