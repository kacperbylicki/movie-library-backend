# Movie Library
The Movie Library is a web application that allows users to browse, comment, and rate movies. It is built using ASP.NET Core and MongoDB Driver, and utilizes a local database to store movie data.

### Prerequisites

- Created AWS Cognito User Pool with Cognito Groups
- Created AWS S3 bucket
- Installed Docker CLI or Docker Desktop,
- .Net Core 6.0.1 and .Net SDK 6.0.1

### Local setup

- Clone this repository ```git clone https://github.com/kacperbylicki/movie-library-dotnet.git```
- Create ```.env``` file in project folder according to ```.env.example``` file, and fill it with your variables
- Run Application, and MongoDB docker containers using ```docker-compose up -d```
- Access the OpenApi docs on ```http://localhost:5085/swagger``` by default
- Test API using Postman or other API testing software
- After all, shutdown docker container using ```docker-compose down```

### Code organization

The project is organized into the following main components:

- Controllers: Handle incoming HTTP requests, process them, and return appropriate responses
- Models: Define the domain models and database models for the application (at the same time).
- DTO: Define data transfer objects, which are used to pass data between the application's layers
- Services: Implement business logic and interact with the database using repositories
- Repositories: Handle data storage and retrieval using MongoDB Driver
- Mappers: Convert between domain models and DTOs

### Features list

- View a list of movies in the collection
- Search for movies by title or year
- Add new movies to the collection
- Edit existing movies in the collection
- Delete movies from the collection
- Create comments
- Update comments
- Delete comments
- Create movies ratings
- Update movies ratings
- Delete movies ratings
- Create account
- Confirm account
- Login into account
- Log off the account

### Features overview

#### Authentication / Accounts

The Movie Library uses Amazon Cognito for user authentication. When a user signs up for the application, their account is created in the Cognito user pool and a confirmation code is sent to their email address. The user must enter the confirmation code to confirm their account. Once the account is confirmed, the user can log in to the application.

To enable Cognito authentication in the project, you will need to set up a user pool in the Amazon Cognito console and provide the necessary configuration values in the project. You can then use the AmazonCognitoIdentityProviderClient class to handle user sign up, sign in, and other authentication-related tasks.

The Movie Library uses Amazon Cognito groups to handle authorization. There are three groups defined in the project: Admin, Moderator, and Viewer.

Users in the Admin group have full access to all resources and actions in the application, including the ability to add, update, and delete movies, as well as edit and delete their own comments and ratings.

Users in the Moderator group have the ability to add and update movies, as well as edit and delete their own comments and ratings. They do not have the ability to delete movies.

Users in the Viewer group can only view movies and leave comments and ratings. They do not have the ability to add, update, or delete movies or delete any comments that are not assign to them. 

Authorization is done in controller using Attributes like in given example:

```
[Authorize(Roles = "Admin, Moderator")]
public IActionResult Update(int id)
{
    // Update movie logic goes here
}
```

To implement the Accounts / Authentication feature, the following components can be used:

 - AccountsController: Handles incoming HTTP requests related to user accounts and returns appropriate responses. This includes actions such as creating a new account, confirming an account, logging in to an account, and logging off an account.
 - User model: Defines the data structure for a user account. This includes properties such as the user's email address, password, and any other relevant information.
 - Tokens model: Defines the data structure for a JWT access token and refresh token.
 - AccountsService: Implements the business logic for the Accounts feature. This includes methods such as:
   - GetSecretHash - is a private method that uses the HMACSHA256 class to generate a hash for a given string using the AppClientSecret from the AWSConfig object and the AppClientId.
   - LoginAsync - is an async method that attempts to log a user in using a CognitoUser object and the provided LoginDto object. It returns a Tokens object on success.
   - GetUserAsync - is an async method that retrieves information about a user based on their access token. It returns a User object.
   - GetUserGroupsAsync - is an async method that retrieves a list of groups that a user with the given email belongs to. It returns a list of group names as strings.
   - RegisterAsync is an async method that registers a new user with the provided RegisterDto object. It returns a SignUpResponse object.
   - ConfirmAccountAsync is an async method that confirms the registration of a new user using the provided ConfirmAccountDto object.
   - RefreshTokenAsync is an async method that refreshes a user's access and refresh tokens using the provided RefreshTokenDto object. It returns a Tokens object.
   - ResendConfirmationCodeAsync is an async method that resends a confirmation code to the user with the given email address.
   - LogoutAsync is an async method that logs a user out by invalidating their access token. It takes in a string representing the user's access token.

#### Movies

The Movies feature allows users to browse and manage their movie collection. It includes the following functionality:

- View a list of movies in the collection
- Search for movies by title or year
- View the details of a particular movie, including its title, year, rating, and plot summary
- Add new movies to the collection
- Edit existing movies in the collection
- Delete movies from the collection 

To implement this feature, the following components are used:

- MoviesController: Handles incoming HTTP requests related to movies and returns appropriate responses. This includes actions such as displaying a list of movies, showing the details of a particular movie, and creating or updating a movie.
- Movie model: Defines the data structure and business logic for a movie. This includes properties such as the title, year, rating, and plot summary.
- MovieRepository class: Implements the IMovieRepository interface using MongoDB Driver. This class is responsible for interacting with the database to store and retrieve movie data.
- MoviesService: Implements the business logic for the Ratings feature. This includes methods such as:
    - ExistsAsync - a private method that returns a boolean indicating whether a movie with the given title exists.
    - GetAllAsync - returns a list of all movies.
    - GetOneByIdAsync - returns a movie with the given id.
    - GetOneByTitleAsync - returns a movie with the given title.
    - CreateAsync - creates a new movie with the information provided in the CreateMovieDto object.
    - UpdateAsync - updates an existing movie with the information provided in the UpdateMovieDto object.
    - DeleteAsync - deletes the movie with the given id.

Additionally Movie Library allows users to upload an image for each movie in their collection. The images are stored in an Amazon S3 bucket. When a user creates a new movie, they can select an image file to upload. The ImagesService class is responsible for handling the image upload process.

To use the ImagesService, you will need to set up an S3 bucket and provide the necessary configuration values in the project. The ImagesService uses the AmazonS3Client class to upload the image to the S3 bucket and returns the URL of the uploaded image. The URL is then stored in the database along with the movie data.

#### Ratings

Users can rate movies on a scale of 1 to 5 stars. The average rating for each movie is displayed on the movie's detail page. The ratings are stored in the database along with the movie data.

To implement this feature, the following components are used:

- RatingsController: Handles incoming HTTP requests related to ratings and returns appropriate responses. This includes actions such as adding a new rating, updating an existing rating, or deleting a rating.
- Rating model: Defines the data structure for a rating. This includes properties such as the movie ID, user ID, and the rating value.
- MoviesRepository: Handles data storage and retrieval for ratings using MongoDB Driver. This repository is used by the RatingsService to store, update, and delete rating data.
- RatingsService: Implements the business logic for the Ratings feature. This includes methods such as:
  - GetOneByIdAsync: Retrieves a rating by its ID and the ID of the movie it belongs to.
  - CreateAsync: Creates a new rating for a movie.
  - UpdateAsync: Updates an existing rating.
  - DeleteAsync: Deletes a rating.

#### Comments

Users can leave comments on movies. The comments are displayed on the movie's detail page, along with the username of the user who left the comment. The comments are stored in the database along with the movie data.

To implement this feature, the following components are used:

- CommentsController: Handles incoming HTTP requests related to comments and returns appropriate responses. This includes actions such as adding a new comment, updating an existing comment, or deleting a comment.
- Comment model: Defines the data structure for a comment. This includes properties such as the movie ID, user ID, and the comment text.
- MoviesRepository: Handles data storage and retrieval for comments using MongoDB Driver. This repository is used by the CommentsService to store, update, and delete comment data.
- CommentsService: Implements the business logic for the Comments feature. This includes methods such as:
  - GetOneByIdAsync: Retrieves a comment by its ID and the ID of the movie it belongs to.
  - CreateAsync: Creates a new comment for a movie.
  - UpdateAsync: Updates an existing comment.
  - DeleteAsync: Deletes a comment.

#### Images
The ImagesService is a class that handles the storage and retrieval of images for a movies library application. It uses Amazon Simple Storage Service (S3) to store the images and generate a unique URL for each image.

To use this service, the following components are required:
 - ImagesService: This is the main class that implements the business logic for storing and retrieving images. It has the following methods:
   - UploadAsync: Accepts a Base64-encoded string representation of an image and stores it in Amazon S3. It returns the URL of the image.
