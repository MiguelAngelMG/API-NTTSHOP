using System;
using System.Collections.Generic;

namespace API_nttshop.Models.Entities;

public partial class ManagementUser
{
    public int PkUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname1 { get; set; } = null!;

    public string? Surname2 { get; set; }

    public string Email { get; set; } = null!;

    public string Languages { get; set; } = null!;
}
