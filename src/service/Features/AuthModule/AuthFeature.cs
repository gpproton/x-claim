// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Axolotl.AspNet.Feature;
using XClaim.Common.Entity;

namespace XClaim.Service.Features.AuthModule;

public class AuthFeature : IFeature {
    public IServiceCollection RegisterModule(IServiceCollection services) => services;

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/account").WithTags("Account");
        group.MapIdentityApi<AccountEntity>();

        return group;
    }
}