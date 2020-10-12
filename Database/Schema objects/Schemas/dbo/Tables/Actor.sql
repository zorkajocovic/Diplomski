CREATE TABLE Actor
  (
    Id INTEGER NOT NULL identity,
    Name NVARCHAR (10) ,
    Surname NVARCHAR (20),
	Deleted bit
  ) ;