
CREATE TABLE Critic
  (
    Id INTEGER NOT NULL identity,
    Name NVARCHAR (15) ,
    Surname NVARCHAR (20) ,
    ReliabilityScore INTEGER,
	Deleted bit
  ) ;