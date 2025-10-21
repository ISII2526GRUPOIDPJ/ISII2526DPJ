SET IDENTITY_INSERT [dbo].[Purchases] ON
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Description], [Street], [Total_price], [PaymentMethodId]) VALUES 
(1, N'Madrid', N'Spain', '2024-01-10', N'Gym equipment', N'Main Street 123', 150.00, 1),
(2, N'Barcelona', N'Spain', '2024-01-12', N'Sports clothing', N'Park Avenue 456', 89.99, 2)
SET IDENTITY_INSERT [dbo].[Purchases] OFF