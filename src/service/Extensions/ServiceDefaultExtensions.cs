// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.IO.Compression;
using Axolotl.AspNet;
using Axolotl.EFCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using XClaim.Common.Context;
using XClaim.Service.Components;
using XClaim.Service.Helpers;
using XClaim.Web.Components.Pages;

namespace XClaim.Service.Extensions;

public static class ServiceDefaultExtensions {
    public static IServiceCollection RegisterDefaultService(this IServiceCollection services) {
        services.AddAntiforgery();
        services.RegisterGenericRepositories(typeof(GenericRepository<>));
        services.RegisterGenericServices();
        services.RegisterFeatures(typeof(Program).Assembly);
        services.AddTransient<FileUploadService>();
        // TODO: Review dependency injection
        services.AddSingleton<HttpClient>(sp => {
            var server = sp.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            string baseAddress = addressFeature!.Addresses.First();
            return new HttpClient { BaseAddress = new Uri(baseAddress) };
        });

        services.AddResponseCompression(options => {
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<GzipCompressionProviderOptions>(options => {
            options.Level = CompressionLevel.Fastest;
        });

        services.AddCors(options => {
            options.AddDefaultPolicy(
                policy => {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
        });
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        return services;
    }

    public static WebApplication RegisterDefaultService(this WebApplication app) {
        switch (app.Environment.IsDevelopment()) {
            case true:
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
                break;
            case false:
                app.UseHsts();
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                break;
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseResponseCompression();
        app.UseCors();
        app.RegisterFeatureEndpoints();
        app.MapRazorComponents<ServerApp>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Home).Assembly);

        return app;
    }
}