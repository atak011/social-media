using System;
using System.Collections.Generic;

#nullable disable

namespace SocialMediaApi.Models
{
    public partial class Post
    {
        public Post()
        {
            Likes = new HashSet<Like>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
