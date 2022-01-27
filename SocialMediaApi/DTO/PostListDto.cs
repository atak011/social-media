using SocialMediaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.DTO
{
    public class PostListDto
    {
        public int id { get; set; }
        public string description { get; set; }
        public User owner { get; set; }
        public string image { get; set; }
        public DateTime created_at { get; set; }
        public bool liked { get; set; }
    }
}
