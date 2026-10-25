
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Common.Models;

namespace OnlineShop.Application.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync(cancellationToken);
        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
