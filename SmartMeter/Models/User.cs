using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class User
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Passwordhash { get; set; } = null!;

    public string Displayname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Lastloginutc { get; set; }

    public bool Isactive { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime RefreshTokenExpiry { get; set; }

    public string Roles { get; set; } = null!;

    public string? Profilepic { get; set; }

    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();
}
