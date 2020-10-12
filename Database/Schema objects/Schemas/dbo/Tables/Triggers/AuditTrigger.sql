CREATE TRIGGER [dbo].[TriggerAudit]
ON [dbo].[Movie]
FOR INSERT
AS
declare @Id int;
declare @Duration int;
declare @Title nvarchar(30);
declare @Director_Id int;
declare @Distributor_Id int;
declare @MetaScore int;
declare @Deleted bit;
declare @audit_action varchar(100);
declare @audit_timestamp varchar(100);

select @Id=i.Id from inserted i;
select @Duration=i.Duration from inserted i;
select @Title=i.Title from inserted i;
select @Distributor_Id=i.Distributor_Id from inserted i;
select @Director_Id=i.Director_Id from inserted i;
select @MetaScore=i.MetaScore from inserted i;
select @Deleted=i.Deleted from inserted i;

set @audit_action='You have been added a new record of Movie.';

insert into Audit
(Id, Duration, Title, Distributor_Id, Director_Id, 
 MetaScore, Deleted, Audit_action, Audit_Timestamp)
values(@Id,@Duration,@Title, @Director_Id,
 @Distributor_Id, @MetaScore, @Deleted, @audit_action, getdate());

PRINT 'AFTER INSERT trigger fired.'
