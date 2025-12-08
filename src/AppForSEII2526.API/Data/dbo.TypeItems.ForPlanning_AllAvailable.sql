SET IDENTITY_INSERT [dbo].[TypeItems] ON

INSERT INTO [dbo].[TypeItems] ([Id], [Name], [ClassId])
VALUES
(1, N'Cardio', 1),
(2, N'Strength', 2);

SET IDENTITY_INSERT [dbo].[TypeItems] OFF
