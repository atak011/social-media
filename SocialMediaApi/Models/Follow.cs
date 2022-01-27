using System;
using System.Collections.Generic;

#nullable disable

namespace SocialMediaApi.Models
{
    public partial class Follow
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Follower { get; set; }
        public virtual User Following { get; set; }
    }
}
