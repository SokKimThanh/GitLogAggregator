--- SOK KIM THANH 16/01/2025
--- Tao csdl để lưu trữ thông tin gitlog trong thời gian thực tập
CREATE DATABASE GitLogAggregatorDB;
GO

USE GitLogAggregatorDB;
GO

-- Tạo bảng InternshipDirectories
CREATE TABLE InternshipDirectories (
    ID INT IDENTITY(1,1),
    InternshipWeekFolder NVARCHAR(255) NOT NULL,
    DateModified DATETIME NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Cập nhật bảng ConfigFiles với khóa ngoại liên kết tới bảng InternshipDirectories
CREATE TABLE ConfigFiles (
    ID INT IDENTITY(1,1),
    ProjectDirectory NVARCHAR(255) NOT NULL,
    InternshipDirectoryId INT NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Weeks INT NOT NULL,
    FirstCommitDate DATETIME NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- ProjectWeeks
CREATE TABLE ProjectWeeks (
    ProjectWeekId INT IDENTITY(1,1),
    WeekStartDate DATETIME,
    WeekEndDate DATETIME,
    InternshipDirectoryId INT NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO
 
-- CombinedCommits
CREATE TABLE Commits (
    CommitId INT IDENTITY(1,1),
    CommitHash NVARCHAR(255) NOT NULL,
    CommitMessage NVARCHAR(MAX) NOT NULL,
    CommitDate DATETIME NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    ProjectWeekId INT NOT NULL,
    Date DATETIME NOT NULL,
    Period NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- ChatbotSummary
CREATE TABLE ChatbotSummary (
    ID INT IDENTITY(1,1),
    CommitId INT NOT NULL,
    Attendance NVARCHAR(255),
    AssignedTasks TEXT,
    ContentResults TEXT,
    SupervisorComments TEXT,
    Notes TEXT,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Ràng buộc khóa chính
ALTER TABLE InternshipDirectories
ADD CONSTRAINT PK_InternshipDirectories PRIMARY KEY (ID);
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT PK_ConfigFiles PRIMARY KEY (ID);
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT PK_ProjectWeeks PRIMARY KEY (ProjectWeekId);
GO
 

ALTER TABLE Commits
ADD CONSTRAINT PK_Commits PRIMARY KEY (CommitId);
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT PK_ChatbotSummary PRIMARY KEY (ID);
GO

-- Ràng buộc khóa ngoại
ALTER TABLE ConfigFiles
ADD CONSTRAINT FK_ConfigFiles_InternshipDirectories FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(ID);
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT FK_ProjectWeeks_InternshipDirectories FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(ID);
GO
 
ALTER TABLE Commits
ADD CONSTRAINT FK_Commits_ProjectWeeks FOREIGN KEY (ProjectWeekId) REFERENCES ProjectWeeks(ProjectWeekId);
GO
 
ALTER TABLE ChatbotSummary
ADD CONSTRAINT FK_ChatbotSummary_Commits FOREIGN KEY (CommitId) REFERENCES Commits(CommitId);
GO

-- Ràng buộc miền giá trị (CHECK constraints)
ALTER TABLE InternshipDirectories
ADD CONSTRAINT CHK_DateModified CHECK (DateModified <= GETDATE());
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT CHK_Weeks CHECK (Weeks > 0);
GO

ALTER TABLE Commits
ADD CONSTRAINT CHK_CommitDate CHECK (CommitDate <= GETDATE());
GO

-- Ràng buộc duy nhất (UNIQUE constraints)
ALTER TABLE Commits
ADD CONSTRAINT UQ_CommitHash UNIQUE (CommitHash);
GO

-- Ràng buộc giá trị mặc định (DEFAULT constraints)

-- InternshipDirectories
ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_DateModified DEFAULT GETDATE() FOR DateModified;
GO

-- ConfigFiles
ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- ProjectWeeks
ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_WeekStartDate DEFAULT GETDATE() FOR WeekStartDate;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_WeekEndDate DEFAULT GETDATE() FOR WeekEndDate;
GO
 

-- Commits
ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- ChatbotSummary
ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

