create PROCEDURE [dbo].[AddMovieFestivalProcedure]
	@festivalId integer,
	@movieId integer
AS

IF NOT EXISTS(SELECT Movie_Id, Festival_Id 
		  FROM MovieFestival
		  WHERE Movie_Id = @movieId and Festival_Id = @festivalId)
	BEGIN

	insert into MovieFestival(Festival_Id, Movie_Id)
	VALUES(@festivalId, @movieId);

	END
