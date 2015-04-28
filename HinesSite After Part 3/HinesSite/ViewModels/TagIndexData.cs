using System.Collections.Generic;
using HinesSite.Models;

namespace HinesSite.ViewModels
{
    public class TagIndexData
    {
        public Tag Tags { get; set; }
        public IEnumerable<Blogpost> Blogposts { get; set; }
    }
}