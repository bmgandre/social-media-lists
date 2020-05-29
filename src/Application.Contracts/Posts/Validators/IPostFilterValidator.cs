﻿using SocialMediaLists.Application.Contracts.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;

namespace SocialMediaLists.Application.Contracts.Posts.Validators
{
    public interface IPostFilterValidator : IModelValidator<SearchPostRequest>
    {
    }
}