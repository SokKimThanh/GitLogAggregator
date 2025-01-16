--- SOK KIM THANH 16/01/2025
--- Tao csdl để lưu trữ thông tin gitlog trong thời gian thực tập
create database GitLogAggregatorDB
go

USE GitLogAggregatorDB;
GO
--So sánh chi tiết:
--Thuộc tính			DateModified								UpdatedAt
--Mục đích				Lưu thời gian thực thể bị thay đổi			Lưu thời gian bản ghi được cập nhật
--Phạm vi				Thư mục, file, dữ liệu						Bản ghi trong cơ sở dữ liệu
--Ví dụ sử dụng			Cập nhật thời gian khi nội dung thay đổi	Cập nhật thời gian khi bản ghi thay đổi
--Tạo bảng InternshipDirectories
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

--ProjectWeeks
CREATE TABLE ProjectWeeks (
    ProjectWeekId INT IDENTITY(1,1),
    ConfigFileId INT NOT NULL,
    InternshipDirectoryId INT NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO
--Commits
CREATE TABLE Commits (
    CommitId INT IDENTITY(1,1),
    CommitHash NVARCHAR(255) NOT NULL,
    CommitMessage TEXT NOT NULL,
    CommitDate DATETIME NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    ProjectWeekId INT NOT NULL,
    CreatedAt DATETIME,
    UpdatedAt DATETIME
);
GO

--ChatbotSummary
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


--ràng buộc khóa chính
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

--ràng buộc khóa ngoại
ALTER TABLE ConfigFiles
ADD CONSTRAINT FK_ConfigFiles_InternshipDirectories FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(ID);
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT FK_ProjectWeeks_ConfigFiles FOREIGN KEY (ConfigFileId) REFERENCES ConfigFiles(ID);
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
--ràng buộc miền giá trị (CHECK constraints)
ALTER TABLE InternshipDirectories
ADD CONSTRAINT CHK_DateModified CHECK (DateModified <= GETDATE());
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT CHK_Weeks CHECK (Weeks > 0);
GO

ALTER TABLE Commits
ADD CONSTRAINT CHK_CommitDate CHECK (CommitDate <= GETDATE());
GO
--ràng buộc duy nhất (UNIQUE constraints)
ALTER TABLE Commits
ADD CONSTRAINT UQ_CommitHash UNIQUE (CommitHash);
GO
--ràng buộc giá trị mặc định (DEFAULT constraints)
ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE InternshipDirectories
ADD CONSTRAINT DF_InternshipDirectories_DateModified DEFAULT GETDATE() FOR DateModified;
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ConfigFiles
ADD CONSTRAINT DF_ConfigFiles_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ProjectWeeks
ADD CONSTRAINT DF_ProjectWeeks_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE Commits
ADD CONSTRAINT DF_Commits_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
GO

ALTER TABLE ChatbotSummary
ADD CONSTRAINT DF_ChatbotSummary_UpdatedAt DEFAULT GETDATE() FOR UpdatedAt;
GO

-- Thêm dữ liệu ban đầu vào bảng mới
INSERT INTO InternshipDirectories (InternshipWeekFolder)
VALUES ('E:\thuctaptotnghiep');
