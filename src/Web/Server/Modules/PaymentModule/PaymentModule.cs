using XClaim.Common.Dtos;

namespace XClaim.Web.Server.Modules.PaymentModule;

public class PaymentModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        services.AddScoped<PaymentService>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        const string name = nameof(Payment);
        var url = $"{Constants.RootApi}/{name.ToLower()}";
        var group = endpoints.MapGroup(url).WithTags(name);

        group.MapGet("/", async (PaymentService sv, [AsParameters] PaymentFilter filter) =>
                await sv.GetAllAsync(null))
            .WithName($"GetAll{name}")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, PaymentService sv) => {
            var result = await sv.GetByIdAsync(id);
            return TypedResults.Ok(result);
        }).WithName($"Get{name}ById").WithOpenApi();

        group.MapPost("/confirmation", async (Payment value, PaymentService sv) => {
            value.CompletedAt = DateTime.Now;
            var result = await sv.UpdateAsync(value);
            return result is null ? Results.NotFound() : TypedResults.Ok(value);
        }).WithName($"Confirm{name}").WithOpenApi();
        
        group.MapPost("/cancel/{id:guid}", async (Guid id, PaymentService sv) => {
            var item = await sv.DeleteAsync(id);
            return item is null ? Results.NotFound() : TypedResults.Ok(item);
        }).WithName($"Cancel{name}").WithOpenApi();

        return group;
    }
}