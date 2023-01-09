using AutoMapper;
using XClaim.Common.Dtos;
using XClaim.Common.Entities;
using XClaim.Web.Server.Data;

namespace XClaim.Web.Server.Modules.BankModule;

public class BankService : GenericService<ServerContext, BankEntity, BankResponse>, IBankService {
    public BankService(ServerContext ctx, IMapper mapper) : base(ctx, mapper) { }
}