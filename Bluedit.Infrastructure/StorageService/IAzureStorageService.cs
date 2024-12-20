﻿using Microsoft.AspNetCore.Http;

namespace Bluedit.Infrastructure.StorageService;

public interface IAzureStorageService
{
    void CreateStorage();
    Task DeleteImage(Guid imageGuid);
    Task<byte[]> GetFileData(Guid imageGuid);
    Task SaveFile(Guid fileName, IFormFile file);
}