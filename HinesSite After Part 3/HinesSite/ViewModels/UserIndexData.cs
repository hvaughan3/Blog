using System.Collections.Generic;
using HinesSite.Models;
using HinesSite.Data;


namespace HinesSite.ViewModels
{
    public class UserIndexData
    {
        public IEnumerable<User> Users { get; set; }
        public virtual IEnumerable<Blogpost> Blogposts { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual IEnumerable<Tag> Tags { get; set; }
    }
}