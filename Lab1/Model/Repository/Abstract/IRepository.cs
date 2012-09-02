using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model.Repository.Abstract
{
    
   //Interface för Repository
    public interface IRepository
    {
        List<User> GetUsers();
        void AddUser(User newUser);
        void RemoveUser(Guid userID);

        List<Post> GetPosts();
        void AddPost(Post newPost);
        void RemovePost(Guid postID);
    }
}
