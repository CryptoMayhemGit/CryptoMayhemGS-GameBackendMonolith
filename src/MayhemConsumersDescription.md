# Mayhem Consumers Description

Consumers are responsible for receiving data from the service bus. 
Projects Mayhem.Item.Consumer.App, Mayhem.Land.Consumer.App, Mayhem.Npc.Consumer.App
update the database status based on the date of the blockchain.

Project Mayhem.Notification.Consumer.App sends emails with invitations to players.

##### Tech stack:
- Serilog - used to logs information in files and console
- Dapper - used for more complex queries
- Azure service bus - used in consumers
- NUnit - testing platform
- Moq - used in tests for mocking objects
- FluentAssertions - for better validation
- MailKit - to send emails