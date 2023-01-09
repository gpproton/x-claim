﻿using XClaim.Common.Base;
using XClaim.Common.Enums;

namespace XClaim.Common.Dtos;

[GenerateAutoFilter]
public class EventResponse : BaseResponse {
    public EventResponse(ClaimResponse? claim, Payment? payment, string? description, EventType type = EventType.Claim) {
        Type = type;
        Claim = claim;
        Payment = payment;
        Description = description;
    }

    public EventType Type { get; set; }
    public ClaimResponse? Claim { get; set; }
    public Payment? Payment { get; set; }
    public string? Description { get; set; }
}