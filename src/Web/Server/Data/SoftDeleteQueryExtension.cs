using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using XClaim.Common.Base;

namespace XClaim.Web.Server.Data;

public static class SoftDeleteQueryExtension {
    // REF: https://www.thereformedprogrammer.net/ef-core-in-depth-soft-deleting-data-with-global-query-filters/#:~:text=You%20can%20add%20a%20soft%20delete%20feature%20to,every%20entity%20class%20you%20want%20to%20soft%20delete.
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData) {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)
            ?.MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall?.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression)filter!);
        entityData.AddIndex(entityData.
            FindProperty(nameof(IBaseEntity.DeletedAt))!);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : class, IBaseEntity {
        Expression<Func<TEntity, bool>> filter = x => x.DeletedAt == null;
        return filter;
    }
}