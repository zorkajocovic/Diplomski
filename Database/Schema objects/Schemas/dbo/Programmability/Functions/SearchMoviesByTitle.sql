
CREATE FUNCTION [dbo].[SearchMovieByTitle]
(
	@searchText nvarchar(30)
)
RETURNS @findedMovies TABLE
(
	[Id] [int] not null,
	[Duration] [int] null,
	[Title] [nvarchar](50) null,
	[Desctiprion] [nvarchar](100) null,
	[Director_Id] [int] not null,
	[Distributor_Id] [int] not null,
	[MetaScore] [decimal] null,
	[Deleted] [bit] not null
)
AS
BEGIN
	 DECLARE
		@countRows int;	

	SELECT @countRows = count(*) FROM Movie 
	WHERE Deleted = 0 and Title like '%' + @searchText + '%'

	DECLARE PROJEKAT_CURSOR CURSOR
	FOR SELECT * FROM Movie
	DECLARE
		@Id int,
		@Duration int,
		@Title nvarchar(50),
		@Desctiprion nvarchar(100),
		@Director_Id int,
		@Distributor_Id int,
		@MetaScore int,
		@Deleted bit;

BEGIN
	OPEN PROJEKAT_CURSOR;
	FETCH NEXT FROM PROJEKAT_CURSOR
	INTO @Id, @Duration, @Title, @Desctiprion, @Director_Id, @Distributor_Id, @MetaScore, @Deleted;

	WHILE @@FETCH_STATUS = 0 
	BEGIN
	IF @countRows = 0
	INSERT INTO @findedMovies (Id, Duration, Title, Desctiprion, Director_Id,
										  Distributor_Id, MetaScore, Deleted)
		SELECT Id, Duration, Title, Desctiprion, Director_Id, Distributor_Id, MetaScore, Deleted
	    FROM Movie
		WHERE Deleted = 0 and Desctiprion like '%' + @searchText + '%' and
			  Id not in(select Id from @findedMovies where Id=Id)
	ELSE
	INSERT INTO @findedMovies (Id, Duration, Title, Desctiprion, Director_Id, 
										  Distributor_Id, MetaScore, Deleted)
		SELECT Id, Duration, Title, Desctiprion, Director_Id, Distributor_Id, MetaScore, Deleted
		FROM Movie 
		WHERE Deleted = 0 and Title like '%' + @searchText + '%' and
			  Id not in(select Id from @findedMovies where Id=Id)

	FETCH NEXT FROM PROJEKAT_CURSOR
	INTO @Id, @Duration, @Title, @Desctiprion, @Director_Id, @Distributor_Id, @MetaScore, @Deleted;

	END

	CLOSE PROJEKAT_CURSOR;
	RETURN
	END
 END
