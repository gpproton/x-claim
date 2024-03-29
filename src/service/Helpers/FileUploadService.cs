// Copyright 2023 - 2023 Godwin peter .O (me@godwin.dev)
//
// Licensed under the MIT License;
// you may not use this file except in compliance with the License.
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Net.Http.Headers;
using XClaim.Common.Responses;

namespace XClaim.Service.Helpers;

public class FileUploadService {
    private readonly IConfiguration _config;

    public FileUploadService(IConfiguration config) {
        _config = config;
    }

    private static string StaticFolderName { get; set; } = "static-files";

    public async Task<List<FileResponse>> UploadFiles(IEnumerable<IFormFile> files) {
        List<FileResponse> uploads = new();
        string? storePath = GetUploadFullPath();

        foreach (IFormFile? file in files) {
            if (file.Length <= 0) {
                continue;
            }

            string? extension = Path.GetExtension(file.FileName);
            string? fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim()
                .ToString();
            string? saveName = Guid.NewGuid() + extension;
            string? filePath = Path.Combine(GetDatePath(), saveName);

            if (!Directory.Exists(storePath)) {
                Directory.CreateDirectory(storePath);
            }

            string? savePath = Path.Combine(storePath, saveName);
            await using FileStream? stream = File.Create(savePath);
            await file.CopyToAsync(stream);
            FileResponse? response = new() {
                Name = fileName,
                Path = filePath,
                Extension = extension
            };
            uploads.Add(response);
        }

        return uploads;
    }

    private static string GetDatePath() {
        return Path.Combine(DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MMMM"),
            DateTime.UtcNow.ToString("dd"));
    }

    private string GetUploadFullPath() {
        return Path.Combine(GetUploadRootPath(), GetDatePath());
    }

    public string GetUploadRootPath() {
        string? filesUploadPath = _config.GetValue<string>("UploadPath");

        return filesUploadPath ?? Path.Combine(Directory.GetCurrentDirectory(), StaticFolderName);
    }
}