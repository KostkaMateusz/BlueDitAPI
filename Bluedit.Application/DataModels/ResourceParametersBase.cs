﻿namespace Bluedit.Application.DataModels;

public abstract class ResourceParametersBase
{
    private const int MaxPageSize = 10;

    private int _pageSize = 5;
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? Fields { get; set; }
    public string OrderBy { get; set; } = string.Empty;
}