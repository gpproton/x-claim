using XClaim.Common.Dtos;

namespace XClaim.Web.Server.Modules.TeamModule;

public class TeamModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        services.AddScoped<TeamService>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        const string name = nameof(Team);
        var url = $"{Constants.RootApi}/{name.ToLower()}";
        var group = endpoints.MapGroup(url).WithTags(name);

        group.MapGet("/", async (TeamService sv, [AsParameters] TeamFilter filter) =>
                await sv.GetAllAsync(null))
            .WithName($"GetAll{name}")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, TeamService sv) => {
            var result = await sv.GetByIdAsync(id);
            return TypedResults.Ok(result);
        }).WithName($"Get{name}ById").WithOpenApi();

        group.MapPost("/", async (Team value, TeamService sv) => {
            await sv.CreateAsync(value);
            return TypedResults.Created($"{url}/{value.Id}", value);
        }).WithName($"Create{name}").WithOpenApi();

        group.MapPut("/", async (Team value, TeamService sv) => {
            var result = await sv.UpdateAsync(value);
            return result is null ? Results.NotFound() : TypedResults.Ok(value);
        }).WithName($"Update{name}").WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, TeamService sv) => {
            var item = await sv.DeleteAsync(id);
            return item is null ? Results.NotFound() : TypedResults.Ok(item);
        }).WithName($"Archive{name}").WithOpenApi();

        return group;
    }
}