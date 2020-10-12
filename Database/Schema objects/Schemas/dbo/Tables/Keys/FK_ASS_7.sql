ALTER TABLE MovieFestivalAward ADD CONSTRAINT FK_ASS_7 
FOREIGN KEY ( AwardFestival_Award_Id, AwardFestival_Festival_Id ) 
REFERENCES AwardFestival ( Award_Id, Festival_Id ) ;
