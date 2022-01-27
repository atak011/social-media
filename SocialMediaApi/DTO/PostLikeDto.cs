using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.DTO
{
    public class PostLikeDto
    {
        public int postId { get; set; }
        public int userId { get; set; }
    }
}
