It is an advanced MVC Core blog site project with admin and member roles, using code first approach, eager loading and linq queries.

This project was written as 3 layers on the basis of N-layer architecture.

The following NuGet packages and frameworks are used in this project.
 AutoMapper,
 Microsoft.AspNetCore.Identity.EntityFrameworkCore, 
 Microsoft.EntityFrameworkCore, 
 Microsoft.EntityFrameworkCore.SqlServer, 
 Microsoft.EntityFrameworkCore.Tools, Microsoft.Extensions.Identity.Stores, 
 SixLabors.ImageSharp,
 SixLabors.ImageSharp.Web
  
Unregistered users can read articles and also filter by categories they want.

Registered users can create articles, create categories, like articles, comment on articles, filter articles by category.

However, the creation of articles, categories and comments becomes active and published after admin approval. In addition, the new user can become an active user and log in to the system after admin approval.

Basic CRUD operations can be done.

You can only register once with the same e-mail address.

If the user wants to change his password, the user has to use a different password than his last 3 passwords.

A separate profile page has been created for each user. Registered and unregistered users can access this page. In this way, you can see the categories that that user follows and the articles they create.

Users can follow as many categories as they want.
