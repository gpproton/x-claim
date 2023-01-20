﻿using System.ComponentModel.DataAnnotations;
using XClaim.Common.Base;

namespace XClaim.Web.Server.Entities;

public sealed class TeamEntity : BaseEntity {
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string Description { get; set; } = string.Empty;
    public CompanyEntity? Company { get; set; }
    public Guid? CompanyId { get; set; }
    public UserEntity? Manager { get; set; }
    public Guid? ManagerId { get; set; }
    public ICollection<UserEntity> Members { get; set; } = default!;
}