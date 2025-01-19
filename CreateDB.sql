--- Cập nhật dữ liệu 18/1/2024
--- chính sửa csdl và thêm bảng mới
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

-- Thêm khóa chính cho bảng InternshipDirectories
ALTER TABLE InternshipDirectories
ADD CONSTRAINT PK_InternshipDirectories PRIMARY KEY (ID);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Thêm ràng buộc giá trị mặc định cho DateModified
ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_DateModified DEFAULT GETDATE() FOR DateModified;
GO
-- Tạo bảng ConfigFiles
CREATE TABLE ConfigFiles (
    ID INT IDENTITY(1,1),
    ProjectDirectory NVARCHAR(255) NOT NULL,
    InternshipDirectoryId INT NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    InternshipStartDate DATETIME NOT NULL,
    InternshipEndDate DATETIME NOT NULL,
    Weeks INT NOT NULL,
    FirstCommitDate DATETIME NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng ConfigFiles
ALTER TABLE ConfigFiles
ADD CONSTRAINT PK_ConfigFiles PRIMARY KEY (ID);
GO

-- Thêm khóa ngoại liên kết đến bảng InternshipDirectories
ALTER TABLE ConfigFiles
ADD CONSTRAINT FK_ConfigFiles_InternshipDirectories FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(ID);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Thêm ràng buộc kiểm tra số tuần (Weeks > 0)
ALTER TABLE ConfigFiles
ADD CONSTRAINT CHK_Weeks CHECK (Weeks > 0);
GO
-- Tạo bảng ProjectWeeks
CREATE TABLE ProjectWeeks (
    ProjectWeekId INT IDENTITY(1,1),
 	ProjectWeekName NVARCHAR(255) NOT NULL,
    WeekStartDate DATETIME,
    WeekEndDate DATETIME,
    InternshipDirectoryId INT NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng ProjectWeeks
ALTER TABLE ProjectWeeks
ADD CONSTRAINT PK_ProjectWeeks PRIMARY KEY (ProjectWeekId);
GO

-- Thêm khóa ngoại liên kết đến bảng InternshipDirectories
ALTER TABLE ProjectWeeks
ADD CONSTRAINT FK_ProjectWeeks_InternshipDirectories FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(ID);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Thêm ràng buộc giá trị mặc định cho WeekStartDate và WeekEndDate
ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_WeekStartDate DEFAULT GETDATE() FOR WeekStartDate;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_WeekEndDate DEFAULT GETDATE() FOR WeekEndDate;
GO
 -- Tạo bảng Commits
CREATE TABLE Commits (
    CommitId INT IDENTITY(1,1),
    CommitHash NVARCHAR(255) NOT NULL,
    CommitMessage NVARCHAR(MAX) NOT NULL,
    CommitDate DATETIME NOT NULL,
    Author NVARCHAR(255) NOT NULL,
	AuthorEmail NVARCHAR(255) NOT NULL,
    ProjectWeekId INT NOT NULL,
    Date DATETIME NOT NULL,
    Period NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng Commits
ALTER TABLE Commits
ADD CONSTRAINT PK_Commits PRIMARY KEY (CommitId);
GO

-- Thêm khóa ngoại liên kết đến bảng ProjectWeeks
ALTER TABLE Commits
ADD CONSTRAINT FK_Commits_ProjectWeeks FOREIGN KEY (ProjectWeekId) REFERENCES ProjectWeeks(ProjectWeekId);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Thêm ràng buộc kiểm tra CommitDate (CommitDate <= GETDATE())
ALTER TABLE Commits
ADD CONSTRAINT CHK_CommitDate CHECK (CommitDate <= GETDATE());
GO

-- Thêm ràng buộc duy nhất cho CommitHash
ALTER TABLE Commits
ADD CONSTRAINT UQ_CommitHash UNIQUE (CommitHash);
GO
-- Tạo bảng CommitGroups
CREATE TABLE CommitGroups (
    CommitGroupId INT IDENTITY(1,1),
    GroupName NVARCHAR(255) NOT NULL,
    TimeRange NVARCHAR(50) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng CommitGroups
ALTER TABLE CommitGroups
ADD CONSTRAINT PK_CommitGroups PRIMARY KEY (CommitGroupId);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE CommitGroups
ADD CONSTRAINT DF_CommitGroups_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE CommitGroups
ADD CONSTRAINT DF_CommitGroups_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Tạo bảng CommitGroupMembers
CREATE TABLE CommitGroupMembers (
    CommitGroupId INT NOT NULL,
    CommitId INT NOT NULL,
    AddedAt DATETIME
);
GO

-- Thêm khóa chính phức hợp cho bảng CommitGroupMembers
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT PK_CommitGroupMembers PRIMARY KEY (CommitGroupId, CommitId);
GO

-- Thêm khóa ngoại liên kết đến bảng CommitGroups
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT FK_CommitGroupMembers_CommitGroups FOREIGN KEY (CommitGroupId) REFERENCES CommitGroups(CommitGroupId);
GO

-- Thêm khóa ngoại liên kết đến bảng Commits
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT FK_CommitGroupMembers_Commits FOREIGN KEY (CommitId) REFERENCES Commits(CommitId);
GO

-- Thêm ràng buộc giá trị mặc định cho AddedAt
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT DF_CommitGroupMembers_AddedAt DEFAULT GETDATE() FOR AddedAt;
GO

-- Tạo bảng ChatbotSummary
CREATE TABLE ChatbotSummary (
    ID INT IDENTITY(1,1),
    CommitGroupId INT NOT NULL,
    Attendance NVARCHAR(255),
    AssignedTasks TEXT,
    ContentResults TEXT,
    SupervisorComments TEXT,
    Notes TEXT,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng ChatbotSummary
ALTER TABLE ChatbotSummary
ADD CONSTRAINT PK_ChatbotSummary PRIMARY KEY (ID);
GO

-- Thêm khóa ngoại liên kết đến bảng CommitGroups
ALTER TABLE ChatbotSummary
ADD CONSTRAINT FK_ChatbotSummary_CommitGroups FOREIGN KEY (CommitGroupId) REFERENCES CommitGroups(CommitGroupId);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO