﻿using System.ComponentModel.DataAnnotations;
using XClaim.Common.Base;
using XClaim.Common.Enums;

namespace XClaim.Web.Server.Entities;

public sealed class EventEntity : BaseEntity {
    public EventType Type { get; set; } = EventType.Claim;
    public ClaimEntity? Claim { get; set; }
    public Guid? ClaimId { get; set; }
    public PaymentEntity? Payment { get; set; }
    public Guid? PaymentId { get; set; }
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
}