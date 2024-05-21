using System;
using System.Collections.Generic;

namespace API_nttshop.Models.Entities;

public partial class User
{
    public int PkUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname1 { get; set; } = null!;

    public string? Surname2 { get; set; }

    public string? Address { get; set; }

    public string? Province { get; set; }

    public string? Town { get; set; }

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string Languages { get; set; } = null!;

    public int Rate { get; set; }

}
