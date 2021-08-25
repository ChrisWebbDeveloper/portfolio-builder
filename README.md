<!-- PROJECT LOGO -->
<p align="center">
  <h3 align="center">portfolio-builder</h3>

  <p align="center">
    A web application to create and configure portfolios
  </p>
</p>


<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

![screenshot](https://user-images.githubusercontent.com/19428849/130439247-c23ab418-2558-440a-8b7f-a1d0ddff3350.jpg)


This is a template for a portfolio website, providing portfolios that can be arranged, resized and configured as the user requires into various categories. Photos can be uploaded, details provided and displayed throughout. Portfolios can be made by users, published, privatised and given a password to hide as need be, giving the user the ability to provide password-protected links to portfolios. A home carousel is also provided, as well as featured portfolios.

Sections for About and Contact are also provided, allowing custome text, links and images to be set. Automatic scrolling and lightbox functionality is also available throughout.

Logging in allows users with different permissions different privileges, from a base user that can build portfolios, a User Manager to create and delete Users, and a Superuser with control to all things including user roles.

### Built With

* [ASP.Net Core 5](https://dotnet.microsoft.com/)
* [MySQL (MariaDB)](https://mariadb.org/)
* [Typescript](https://www.typescriptlang.org/)
* [SASS](https://sass-lang.com/)
* [Bootstrap 4](https://getbootstrap.com/)
* [Gulp](https://gulpjs.com/)
* [IIS](https://www.iis.net/)


<!-- GETTING STARTED -->
## Getting Started

As this was built in ASP.Net Core, to get this set up Visual Studio is a must. Some basic steps are needed and provided below to get started, but beyond that is up to you.


### Prerequisites

You will need the following in order to build this project:
* Visual Studio with ASP.Net Core 5 available
* IIS or Docker
* An empty database to run from (or multiple for different environments)
* NPM


### Installation

1. Open the PortfolioBuilder.sln file in Visual Studio
2. In the project's terminal move into the ProjectBuilder folder
   ```sh
   cd .\PortfolioBuilder
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Open the Task Manager in Visual Studio - this can be found using the search bar
5. Run the 'initialise' gulp task. This should install libraries into wwwroot\lib, and create the necessary js and css files
6. In the appsettings.json (also relevant Development, Staging and Production environments if you so choose), set your connection details for the empty database(s)
7. In the project's terminal, create the database:
   ```sh
   update-database
   ```
8. Build the project using `Ctrl + Shift + B`
9. Start without Debugging using `Ctrl + F5` (or with Debugging with just `F5`) using your preferred server, whether that is IIS or Docker
10. The project should now be open in a browser


<!-- USAGE EXAMPLES -->
## Usage

Some dummy info is provided to provide layout on first instance. This can be seen on the home page with the placeholder images and portfolios, as well as in the About and Contact sections.

To alter anything, you will need to login. From the base url add `/login` (eg. https://localhost:44379/login). The following users are provided on startup:

* Superuser - User: `system@portfoliobuilder.com`, Password: `System123`
* User Manager - User: `usermanager@portfoliobuilder.com`, Password: `User123`

From either of these, users can be added or deleted, and roles set accordingly.

When logged in, any user can go through and alter portfolios and photos. Photos can be uploaded, with a title and description given. They can be added to portfolios, resized and displayed as need be.

Portfolios can be arranged and resized as well, included in categories, featured, privatised and given passwords. They can also be kept as drafts by not publishing them.

About allows the user to set a title and description. Images can also be added that automatically resize to fit the height of the page/text, and scroll automatically through them.

Contact information can also be provided, automatically linking as need be.

For all of this, it is best to play around and see what you can do and what you can build!


<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
* [jQuery](https://jquery.com/)
* [jQuery Validation](https://jqueryvalidation.org/)
* [Font Awesome](https://fontawesome.com)
* [Slick Carousel](https://kenwheeler.github.io/slick/)
* [Best-README-Template](https://github.com/othneildrew/Best-README-Template)
* [Docker](https://www.docker.com/)

Demo Photos from Unsplash
* [Photo by Alexander Andrews](https://unsplash.com/photos/Y4rY3_BM5Io)
* [Photo by Aranprime](https://unsplash.com/photos/hwzLBk3079Q)
* [Photo by Carlos Lindner](https://unsplash.com/photos/qjqcKJXO5hs)
* [Photo by William Moreland](https://unsplash.com/photos/G0Ot7IsL-_I)
* [Photo by Alberto Restifo](https://unsplash.com/photos/HYA9Ak06qR8)
* [Photo by Logan Weaver](https://unsplash.com/photos/h_abdKXlMFM)
* [Photo by Jay Wennington](https://unsplash.com/photos/sl1-IazYY7I)
* [Photo by Bailey Zindel](https://unsplash.com/photos/NRQV-hBF10M)


<!-- MARKDOWN LINKS & IMAGES -->
[screenshot]: README/screenshot.png
