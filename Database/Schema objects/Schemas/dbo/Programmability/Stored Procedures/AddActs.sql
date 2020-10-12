
create PROCEDURE [dbo].[AddActsProcedure]
	@actorId integer,
	@movieId integer
	AS
BEGIN

IF NOT EXISTS(SELECT Movie_Id, Actor_Id 
		  FROM Acts
		  WHERE Movie_Id = @movieId and Actor_Id = @actorId)
	BEGIN

	insert into Acts(Actor_Id, Movie_Id)
	VALUES(@actorId, @movieId);

	END
END