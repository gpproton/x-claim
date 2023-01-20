using XClaim.Common.Dtos;
using XClaim.Common.Wrappers;

namespace XClaim.Common.HTTP;

public interface ICurrencyService {
    Task<PagedResponse<List<CurrencyResponse>>> GetAllAsync(object? query = null);
    
    Task<Response<CurrencyResponse>> GetByIdAsync(Guid id);
    
    Task<Response<CurrencyResponse>> CreateAsync(CurrencyResponse category);
    
    Task<Response<CurrencyResponse>> UpdateAsync(CurrencyResponse category);
    
    Task<Response<CurrencyResponse>> ArchiveAsync(Guid id);
    
    Task<Response<List<CurrencyResponse>>> ArchiveRangeAsync(List<Guid> ids);
}