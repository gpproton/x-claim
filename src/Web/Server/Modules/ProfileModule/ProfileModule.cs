using System.Security.Claims;
using Newtonsoft.Json;
using XClaim.Common.Dtos;

namespace XClaim.Web.Server.Modules.ProfileModule;

public class ProfileModule : IModule {
    record ProfileResponse(bool Valid = false, string Message = "", Profile? Data = null);

    public IServiceCollection RegisterApiModule(IServiceCollection services) {

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        const string name = "Profile";
        var url = $"{Constants.RootApi}/{name.ToLower()}";
        var group = endpoints.MapGroup(url).WithTags(name);

        group.MapGet("/account", async (context) => {
            bool isAuth = context.User.Identity?.IsAuthenticated ?? false;
            if (!isAuth) await context.Response.WriteAsJsonAsync(
                new ProfileResponse(false, "Not Authenticated", null)
                );

            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var fullName = context.User.FindFirst(ClaimTypes.Name)?.Value;
            var phone = context.User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            if (email is null)
                await context.Response.WriteAsJsonAsync(
                    new ProfileResponse(false, "Email address is invalid", null)
                    );

            var profile = new Profile(email!, fullName!, phone!);
            // context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(
                new ProfileResponse(false, "Success", profile)
                );
        }).WithName("AccountProfile").WithOpenApi();

        return group;
    }
}