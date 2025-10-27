SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [CreatedDate], [Description], [HealthIssues], [Name], [TotalPrice], [Weeks], [PaymentMethodId]) VALUES 
(1, '2024-01-15', N'Monthly fitness plan', N'None', N'Fitness Plan', 50.00, 4, 1),
(2, '2024-01-16', N'Yoga and meditation', N'Back problems', N'Relax Plan', 40.00, 8, 2)
SET IDENTITY_INSERT [dbo].[Plans] OFF