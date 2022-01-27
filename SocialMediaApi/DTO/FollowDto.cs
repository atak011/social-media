using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.DTO
{
    public class FollowDto
    {
        public int followerId { get; set; }
        public int followingId { get; set; }
    }
}
