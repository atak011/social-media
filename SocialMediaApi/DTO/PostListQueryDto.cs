using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.DTO
{
    public class PostListQueryDto
    {
        public int userId { get; set; }
        public List<int> postIds { get; set; }
    }
}
