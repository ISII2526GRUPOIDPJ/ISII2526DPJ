SET IDENTITY_INSERT [dbo].[Classes] ON

INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price])
VALUES
(1, N'Cardio Blast', 10, DATEADD(DAY, 1, CAST(CONVERT(date, GETDATE()) + ' 07:00' AS datetime)), CAST(10.00 AS Decimal(10,2)));

INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price])
VALUES
(2, N'Strength Training', 10, DATEADD(DAY, 2, CAST(CONVERT(date, GETDATE()) + ' 08:00' AS datetime)), CAST(20.00 AS Decimal(10,2)));

SET IDENTITY_INSERT [dbo].[Classes] OFF
