﻿using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Services.Users;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class ProfilePostsViewComponent : AppViewComponent
    {
        private readonly IUserService _userService;
        private readonly IProfileModelFactory _profileModelFactory;

        public ProfilePostsViewComponent(IUserService userService, IProfileModelFactory profileModelFactory)
        {
            this._userService = userService;
            this._profileModelFactory = profileModelFactory;
        }

        public IViewComponentResult Invoke(int userProfileId, int pageNumber)
        {
            var user = _userService.GetUserById(userProfileId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var model = _profileModelFactory.PrepareProfilePostsModel(user, pageNumber);
            return View(model);
        }
    }
}
