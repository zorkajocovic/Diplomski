
CREATE TABLE Review
  (
    Id INTEGER NOT NULL identity,
    Text text ,
    Critic_Id INTEGER NOT NULL ,
    Rate      INTEGER ,
    Movie_Id  INTEGER NOT NULL,
	Deleted bit
  ) ;