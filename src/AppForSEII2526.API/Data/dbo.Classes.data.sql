SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (3, N'Yoga Basics', 2, N'2025-10-10 11:30:00', CAST(12.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (4, N'Cardio Blast', 5, N'2025-10-13 17:00:00', CAST(10.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (5, N'Strength Training', 4, N'2025-10-14 15:00:00', CAST(20.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (6, N'Meditation', 6, N'2025-10-17 10:00:00', CAST(8.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (7, N'Zumba', 4, N'2025-10-20 18:30:00', CAST(12.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Classes] ([Id], [Name], [Capacity], [Date], [Price]) VALUES (8, N'Kick-Boxing', 5, N'2025-10-18 17:30:00', CAST(19.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Classes] OFF
