create PROCEDURE [dbo].[AddBelongsToGenreProcedure]
	@genreId integer,
	@movieId integer
AS

IF NOT EXISTS(SELECT Movie_Id, Genre_Id 
		  FROM Belongs_to
		  WHERE Movie_Id = @movieId and Genre_Id = @genreId)
	BEGIN

	insert into Belongs_to(Genre_Id, Movie_Id)
	VALUES(@genreId, @movieId);

	END
