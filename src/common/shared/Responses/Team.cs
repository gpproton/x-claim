// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Axolotl.Response;

namespace XClaim.Common.Responses; 

public class Team : BaseResponse<Guid> {
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string Description { get; set; } = string.Empty;
    public Company? Company { get; set; }
    public int? CompanyId { get; set; }
    public User? Manager { get; set; }
    public Guid? ManagerId { get; set; }
    public ICollection<User>? Members { get; set; }
}