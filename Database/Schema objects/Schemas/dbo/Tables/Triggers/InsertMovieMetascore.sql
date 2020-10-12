
create TRIGGER [dbo].[InsertMovieMetascore]
ON [dbo].[Review]
after update
AS
BEGIN

	declare @movieId int;
	declare @sumMetascore decimal(18,2);
	declare @oldMetascore decimal(18,2);
	declare @sum int;
	declare @triggeredDatetime datetime2;
	declare @approvalDate datetime2;
	declare @approved bit;
	declare @reviewId int;
	declare @seconds int;

	set @sumMetascore = 0;
	set @seconds = 3;
	set @sum = 0;
	
	select @movieId = review.Movie_Id,
			@reviewId =  review.Id,
			@approved = review.Approved,
			@approvalDate = review.ApprovalDateTime
	from inserted review;

	set @triggeredDatetime = getdate();

	select @sum = sum(c.ReliabilityScore)
	from Review r, Critic c
	where r.Critic_Id = c.Id and r.Movie_Id = @movieId and c.Deleted = 0 and r.Deleted = 0;
	
	select @oldMetascore = MetaScore from Movie where Id = @movieId;
	select @sumMetascore = dbo.CalculateMetascore(@sum, @movieId);

	UPDATE Movie 
    SET MetaScore = (case when (DATEDIFF(second, @approvalDate, @triggeredDatetime) < @seconds) and @approved = 1
                              THEN @sumMetascore else @oldMetascore end)
							  where Id = @movieId;

END	