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
using Axolotl.Enums;
using XClaim.Common.Entity;
using XClaim.Common.Responses;

namespace XClaim.Service.Features.ResourceModule; 

public class EventFeature : GenericFeature<EventFeature> {
    public override IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        IEndpointRouteBuilder? group = SetupGroup<EventFeature, EventEntity, EventResponse, Guid>(
            endpoints,
            new FeatureState(
                new List<RouteState> {
                    new(RouteType.GetAll),
                    new(RouteType.GetById)
                },
                Name: "Event",
                Path: "event"
            ));

        return group;
    }
}