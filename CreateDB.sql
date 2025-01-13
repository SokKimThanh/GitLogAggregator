USE GitLogAggregatorDB;
GO

CREATE TABLE ConfigFiles (
    Id INT PRIMARY KEY IDENTITY,
    ProjectDirectory NVARCHAR(255) NOT NULL,
    InternshipWeekFolder NVARCHAR(255) NOT NULL,
    Author NVARCHAR(100),
    StartDate DATETIME,
    EndDate DATETIME,
    Weeks INT,
    FirstCommitDate DATETIME
);
GO
