﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;

        public PostService(DataContext dataContext)
            => _dataContext = dataContext;


        public async Task<List<Post>> GetPostsAsync()
            => await _dataContext.Posts.ToListAsync();

        public async Task<Post> GetPostByIdAsync(long Id)
            => await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(long postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);

            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(long postId, string getUserId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId && x.UserId == getUserId);

            return post != null ? true : false;
        }
    }
}