﻿using System;
using System.Collections.Generic;

namespace AspNetCoreCookieAuthentication.Areas.Identity.Data;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
}
