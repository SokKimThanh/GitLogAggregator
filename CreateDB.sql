-- Cập nhật dữ liệu ngày 20/01/2025
-- tách quan hệ tác giả và config, 1 dự án có nhiều tác giả, 1 tác giả tham gia nhiều dự án
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
    ConfigID INT IDENTITY(1,1) PRIMARY KEY, -- Khóa chính, tự động tăng
    ProjectDirectory NVARCHAR(255) NOT NULL, -- Đường dẫn thư mục dự án
    InternshipDirectoryId INT NOT NULL, -- ID thư mục thực tập
    InternshipStartDate DATETIME NOT NULL, -- Ngày bắt đầu thực tập
    InternshipEndDate DATETIME NOT NULL, -- Ngày kết thúc thực tập
    Weeks INT NOT NULL, -- Số tuần thực tập
    FirstCommitDate DATETIME NOT NULL, -- Ngày commit đầu tiên
	FirstCommitAuthor NVARCHAR(255) NOT NULL, -- Tác giả đầu tiên commit
    CreatedAt DATETIME, -- Ngày tạo bản ghi
    UpdatedAt DATETIME -- Ngày cập nhật bản ghi
);
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

-- Tạo bảng Authors
CREATE TABLE Authors (
    AuthorID INT IDENTITY(1,1), -- Khóa chính, tự động tăng
    AuthorName NVARCHAR(255) NOT NULL, -- Tên tác giả 
	AuthorEmail NVARCHAR(255) NOT NULL UNIQUE,-- Email (duy nhất)
    CreatedAt DATETIME DEFAULT GETDATE(), -- Ngày tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE() -- Ngày cập nhật bản ghi
);
GO 
-- Thêm ràng buộc Primary Key cho cột AuthorID
ALTER TABLE Authors
ADD CONSTRAINT PK_Authors PRIMARY KEY (AuthorID);
GO

-- Tạo bảng ConfigAuthors
CREATE TABLE ConfigAuthors (
    ConfigID INT NOT NULL, -- Khóa ngoại tham chiếu đến ConfigFiles
    AuthorID INT NOT NULL, -- Khóa ngoại tham chiếu đến Authors
    CreatedAt DATETIME DEFAULT GETDATE(), -- Ngày tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE() -- Ngày cập nhật bản ghi
);
GO
-- Thêm khóa chính kết hợp cho bảng ConfigAuthors
ALTER TABLE ConfigAuthors
ADD CONSTRAINT PK_ConfigAuthors PRIMARY KEY (ConfigID, AuthorID);
GO
-- Thêm khóa ngoại tham chiếu đến bảng ConfigFiles
ALTER TABLE ConfigAuthors
ADD CONSTRAINT FK_ConfigAuthors_ConfigFiles
FOREIGN KEY (ConfigID) REFERENCES ConfigFiles(ConfigID) ON DELETE CASCADE;
GO

-- Thêm khóa ngoại tham chiếu đến bảng Authors
ALTER TABLE ConfigAuthors
ADD CONSTRAINT FK_ConfigAuthors_Authors
FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE CASCADE;
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
-- Tạo bảng CommitPeriods
CREATE TABLE CommitPeriods (
    PeriodID INT IDENTITY(1,1),
    PeriodName NVARCHAR(255) NOT NULL,
    PeriodDuration NVARCHAR(50) NOT NULL,
    PeriodStartDate DATETIME NOT NULL,
    PeriodEndDate DATETIME NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

-- Thêm khóa chính cho bảng CommitPeriods
ALTER TABLE CommitPeriods
ADD CONSTRAINT PK_CommitPeriods PRIMARY KEY (PeriodID);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE CommitPeriods
ADD CONSTRAINT DF_CommitPeriods_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE CommitPeriods
ADD CONSTRAINT DF_CommitPeriods_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Tạo bảng CommitGroupMembers
CREATE TABLE CommitGroupMembers (
    PeriodID INT NOT NULL,
    CommitId INT NOT NULL,
    AddedAt DATETIME
);
GO

-- Thêm khóa chính phức hợp cho bảng CommitGroupMembers
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT PK_CommitGroupMembers PRIMARY KEY (PeriodID, CommitId);
GO

-- Thêm khóa ngoại liên kết đến bảng CommitPeriods
ALTER TABLE CommitGroupMembers
ADD CONSTRAINT FK_CommitGroupMembers_CommitPeriods FOREIGN KEY (PeriodID) REFERENCES CommitPeriods(PeriodID);
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
    PeriodID INT NOT NULL,
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

-- Thêm khóa ngoại liên kết đến bảng CommitPeriods
ALTER TABLE ChatbotSummary
ADD CONSTRAINT FK_ChatbotSummary_CommitPeriods FOREIGN KEY (PeriodID) REFERENCES CommitPeriods(PeriodID);
GO

-- Thêm ràng buộc giá trị mặc định cho CreatedAt và UpdatedAt
ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO