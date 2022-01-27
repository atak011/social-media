using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.DTO;
using SocialMediaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SocialMediaController : ControllerBase
    {

        [HttpGet]
        [Route("index")]
        public string index()
        {
            return "start";
        }
   
        [HttpGet]
        [Route("UserPosts")]
        public List<Post> getUserPosts (int userId)
        {
            List<Post> posts = new List<Post>();

            using (var ctx = new SocialMediaContext())
            {
                string userSql = "select * from user where id= " + userId;
                User usrControl = ctx.Users.FromSqlRaw(userSql).FirstOrDefault();
                //önce gönderilen id ile kullanıcı varmı yokmu kontrol ediliyor.
                if (usrControl != null)
                {
                    //id ile kullanıcı var ise kullanıcının postları listeleniyor.
                    posts = ctx.Posts.Where(x => x.UserId == userId).ToList();
                }
            }
            return posts;
        }

        [HttpPost]
        [Route("SigUp")]
        public bool sigup(SignupDto signupDto)
        {
            bool result = false;
            using (var ctx = new SocialMediaContext())
            {

                string userSql = "select * from user where username='"+ signupDto.username + "'";
                User usrControl = ctx.Users.FromSqlRaw(userSql).FirstOrDefault();
                if (usrControl == null)
                {
                    //aynı kullanıcı adi ile daha önce kayıt olup olmadıgı kontrol ediliyor.
                    User newUsr = new User();
                    newUsr.Email = signupDto.email;
                    newUsr.Username = signupDto.username;
                    newUsr.CreatedAt = DateTime.Now;
                    ctx.Users.Add(newUsr);
                    int snc = ctx.SaveChanges();
                    if (snc == 1)
                        result = true;
                }
            }
            return result;
        }

        [HttpPost]
        [Route("Like")]
        public bool likePost(PostLikeDto postLikeDto)
        {
            bool result = false;
            using (var ctx = new SocialMediaContext())
            {
                string postSql = "select * from post where id=" + postLikeDto.postId;
                Post postControl = ctx.Posts.FromSqlRaw(postSql).FirstOrDefault() ;
                //gönderdiği id ile post olup olmadıgı kontrol ediliyor.
                if (postControl == null)
                {
                    //post var ise daha önce like atıp atmadıgı kontrol ediyor. eger like atmış ise aynı metot çalışırsa like attıgı posttan like kaldırılıyor.
                    string likeSql = "select *  from like where post_id=" + postLikeDto.postId + " and  user_id = " + postLikeDto.userId;
                    Like likeCtr = ctx.Likes.FromSqlRaw(likeSql).FirstOrDefault();
                    if(likeCtr == null)
                    {
                        //eger like bulunmuyor ise eklenecek.
                        Like newLike = new Like();
                        newLike.PostId = postLikeDto.postId;
                        newLike.UserId = postLikeDto.userId;
                        newLike.CreatedAt = DateTime.Now;
                        ctx.Likes.Add(newLike);                 
                    }
                    else
                    {
                        // daha önce like attıgı için tekrar post ettiğinde like kaldırılıyor.

                        ctx.Likes.Remove(likeCtr);                     
                    }

                    int snc = ctx.SaveChanges();
                    if (snc == 1)
                        result = true;
                }
            }
            return result;
        }

        [HttpPost]
        [Route("Follow")]
        public bool followUnfollow(FollowDto followDto)
        {
            bool result = false;
            using (var ctx = new SocialMediaContext())
            {
                string followSql = "select * from follow where follower_id =" + followDto.followerId + "  and following_id=" + followDto.followingId;
                Follow followCtr = ctx.Follows.FromSqlRaw(followSql).FirstOrDefault();
                if (followCtr == null)
                {
                    // kayıt yok ise follow işlemi gerçekleşiyor
                    Follow newFollow = new Follow();
                    newFollow.FollowerId = followDto.followerId;
                    newFollow.FollowingId = followDto.followingId;
                    newFollow.CreatedAt = DateTime.Now;
                    ctx.Follows.Add(newFollow);
                }
                else
                {
                    // daha önce takip ettiği için unfollow işlemi gerçekleşiyor

                    ctx.Follows.Remove(followCtr);
                }

                int snc = ctx.SaveChanges();
                if (snc == 1)
                    result = true;
            }
            return result;
        }

        [HttpPost]
        [Route("Posts")]
        public List<PostListDto> getPosts(PostListQueryDto queryDto)
        {
            List<PostListDto> result = new List<PostListDto>();

            using (var ctx = new SocialMediaContext())
            {
                foreach (int postId in queryDto.postIds)
                {
                    // parametre olarak gelen post_ids tek tek post tablosundan sorgulanacak.     
                    
                    string postSql = "select * from post where id= " + postId;
                    Post post = ctx.Posts.FromSqlRaw(postSql).FirstOrDefault();
                    if (post != null)
                    {
                        //gönderilen post id var ise postun sahibi user bilgisi çekilecek
                        string userSql = "select * from user where id= " + post.UserId;
                        User ownerUser = ctx.Users.FromSqlRaw(userSql).FirstOrDefault();

                        //sorgulayan kişi bu postu begenip begenmediği bilgisi çekilecek
                        bool liked = false;
                        string likeSql = "select * from like where post_id= " + post.Id + "  and user_id ="+ queryDto.userId;
                        Like like = ctx.Likes.FromSqlRaw(likeSql).FirstOrDefault();
                        if(like != null)
                        {
                            // kullanıcı bu postu begenmiş ise liked değişkeni true olarak güncelleniyor.
                            liked = true;
                        }


                        // tüm datalar toplandı şimdi postlist içine obje olarak ekleniyor.

                        result.Add(new PostListDto() { 
                            liked = liked,
                            created_at = post.CreatedAt,
                            description = post.Description,
                            id = post.Id,
                            image = post.Image,
                             owner = ownerUser
                        });
                    }
                }
            }

            return result;
        }

        [HttpPost]
        [Route("MergePost")]
        public List<Post> mergePosts(List<List<Post>> postList)
        {
            List<Post> result = new List<Post>();
            List<Post> listCreated = new List<Post>();
            List<Post> listId = new List<Post>();
            foreach (List<Post> itemList in postList)
            {
                foreach (Post item in itemList)
                {
                    if (!listCreated.Contains(item))
                    {
                        // dönecek olacak list post objesinde  post yok ise eklenecek. mükerrer kayıdın önüne geçmek için
                        listCreated.Add(item);
                    }
                }
            }

            // şuan elimizde uniq mükerrer olmayan bir liste var. şimdi sıralama işlemi yapılacak

            while (listCreated.Count > 0)
            {
                DateTime currentCreated = new DateTime();
                int currentPostId = 0;
                for (int i = 0; i < listCreated.Count; i++)
                {
                    if (listCreated[i].CreatedAt < currentCreated)
                    {
                        currentCreated = listCreated[i].CreatedAt;
                        currentPostId = listCreated[i].Id;
                    }
                }
                Post removePost = listCreated.FirstOrDefault(x => x.Id == currentPostId);

                listId.Add(removePost); //id kontrol edilecek olan objeye ekleniyor
                listCreated.Remove(removePost); //kontrol ettiğimiz listeden çıkartıyoruz.                
            }

            //created göre sıralandı şimdi aynı tarih olan ve id si önce olan sıralanacak

            while (listId.Count > 0)
            {
                DateTime firsCreated = new DateTime();
                int currentPostId = 0;
                for (int i = 0; i < listId.Count; i++)
                {
                    if (i == 0)
                    {
                        firsCreated = listId[i].CreatedAt;    //ilk kayıdı aldık bundan sonra aynı tarih olanlara bakılacak
                        currentPostId = listId[i].Id;
                    }
                    else
                    {
                        if(firsCreated == listId[i].CreatedAt && currentPostId > listId[i].Id)
                        {
                            //ilk kayıt sonrası diger kayıtlarda tarih aynı ise ve id gelen kayıttan büyük ise firsCreated ve id güncellencek.
                            firsCreated = listId[i].CreatedAt;    
                            currentPostId = listId[i].Id;
                        }
                    }
                }

                Post removePost = listId.FirstOrDefault(x => x.Id == currentPostId);

                result.Add(removePost); //en son dönecek olan değişkene atanıyor
                listId.Remove(removePost); //kontrol ettiğimiz listeden çıkartıyoruz.   
            }

            return result;
        }
    }
}
