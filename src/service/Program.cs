// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using XClaim.Service.Data;
using XClaim.Service.Extensions;
using XClaim.Web.Components.Extensions;
using XClaim.Web.Shared;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.RegisterDataContext();
builder.Services.AddHostedService<MigrationService>();
builder.Services.RegisterSwaggerService();
builder.Services.RegisterDefaultService();
builder.Services.RegisterAuthenticationService();
builder.Services.RegisterCoreAdmin();
builder.Services.RegisterSharedBlazorServices();
builder.Services.RegisterServerRazorExtensions();
builder.Services.RegisterServerBlazorState();

WebApplication app = builder.Build();
app.RegisterSwaggerService();
app.RegisterFileUploadService();
app.RegisterDefaultService();
app.RegisterCoreAdmin();

app.Run();
