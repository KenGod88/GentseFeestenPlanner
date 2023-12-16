USE [GentseFeesten]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
    [UserId] INT PRIMARY KEY IDENTITY(1,1),
    [FirstName] NVARCHAR(50),
    [LastName] NVARCHAR(50),
    [DailyBudget] DECIMAL(18, 2)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Events](
    [EventId] UNIQUEIDENTIFIER PRIMARY KEY,
    [Title] NVARCHAR(255),
    [Description] NVARCHAR(MAX),
    [StartTime] DATETIME,
    [EndTime] DATETIME,
    [Price] DECIMAL(18, 2)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DayPlans](
    [DayPlanId] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT,
    [Date] DATE,
    CONSTRAINT FK_DayPlans_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DayPlanEvents](
    [DayPlanId] INT,
    [EventId] UNIQUEIDENTIFIER,
    CONSTRAINT PK_DayPlanEvents PRIMARY KEY (DayPlanId, EventId),
    CONSTRAINT FK_DayPlanEvents_DayPlanId FOREIGN KEY (DayPlanId) REFERENCES DayPlans(DayPlanId),
    CONSTRAINT FK_DayPlanEvents_EventId FOREIGN KEY (EventId) REFERENCES Events(EventId)
) ON [PRIMARY]
GO