SET IDENTITY_INSERT [dbo].[Classes] ON

INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price])
VALUES
(1, N'Cardio Blast', 10, DATEADD(DAY, 1, CAST(GETDATE() AS DATETIME)), CAST(10.00 AS Decimal(10,2))),
(2, N'Strength Training', 10, DATEADD(DAY, 2, CAST(GETDATE() AS DATETIME)), CAST(20.00 AS Decimal(10,2)));

SET IDENTITY_INSERT [dbo].[Classes] OFF
