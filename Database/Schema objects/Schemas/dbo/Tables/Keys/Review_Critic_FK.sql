ALTER TABLE Review ADD CONSTRAINT Review_Critic_FK FOREIGN KEY ( Critic_Id ) REFERENCES Critic ( Id ) ;
