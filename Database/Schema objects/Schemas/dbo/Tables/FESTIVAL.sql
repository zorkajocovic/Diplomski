
CREATE TABLE Festival
  (
    Id INTEGER NOT NULL identity,
    Name NVARCHAR (20) ,
    Place_Id INTEGER NOT NULL ,
    Date   varchar(15),
	Deleted bit
  ) ;