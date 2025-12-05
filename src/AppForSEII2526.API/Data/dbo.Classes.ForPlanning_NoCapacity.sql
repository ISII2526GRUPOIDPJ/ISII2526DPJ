SET IDENTITY_INSERT [dbo].[Classes] ON

INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price])
VALUES
(3, N'Yoga', 0, DATEADD(DAY, 2, CAST(CONVERT(date, GETDATE()) + ' 09:00' AS datetime)), CAST(12.00 AS Decimal(10,2)));

SET IDENTITY_INSERT [dbo].[Classes] OFF
