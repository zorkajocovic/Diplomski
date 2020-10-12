

CREATE TABLE Movie
  (
    Id       INTEGER NOT NULL identity,
    Duration INTEGER ,
    Title NVARCHAR (50) ,
    Desctiprion text ,
    Director_Id    INTEGER NOT NULL ,
    Distributor_Id INTEGER NOT NULL ,
    MetaScore      decimal(18,2),
	Deleted bit
  ) ;