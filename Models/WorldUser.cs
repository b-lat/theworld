using Microsoft.AspNet.Identity.EntityFramework;

namespace TheWorld.Models
{
    using System;

    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}