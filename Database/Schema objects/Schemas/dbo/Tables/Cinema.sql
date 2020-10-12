
CREATE TABLE Cinema
  (
    Id INTEGER NOT NULL identity,
    Name NVARCHAR (20) ,
    Place_Id INTEGER NOT NULL,
	Deleted bit
  ) ;