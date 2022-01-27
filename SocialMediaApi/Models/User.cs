using System;
using System.Collections.Generic;

#nullable disable

namespace SocialMediaApi.Models
{
    public partial class User
    {
        public User()
        {
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
