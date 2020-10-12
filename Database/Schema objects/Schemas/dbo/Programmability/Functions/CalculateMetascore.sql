CREATE FUNCTION [dbo].[CalculateMetascore]
(
	@rate int,
	@reliabilityScore int,
	@countReviews int
)
RETURNS decimal
AS

DECLARE
	@rateReliability as decimal;
	
	select @rateReliability = sum(r.Rate * c.ReliabilityScore)
	from Review r, Critic c
	where r.Critic_Id = c.Id and r.Movie_Id = @movieId and c.Deleted = 0 and r.Deleted = 0;

	RETURN @rateReliability/@sum;

END

